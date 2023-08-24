using BlogWeb.Infrastuctures.JwtBuilder;
using CoreLayer.DTOs.User;
using CoreLayer.DTOs.UserToken;
using CoreLayer.Services.User;
using CoreLayer.Services.UserRole;
using CoreLayer.Services.UserToken;
using CoreLayer.Utilities;
using CoreLayer.Utilities.SecurityUtil;
using DataLayer.Entities.Users;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.IdentityModel.Tokens;
using System.ComponentModel.DataAnnotations;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using BlogWeb.Utilities.GoogleRecapcha;

namespace BlogWeb.Pages.Auth
{
    [BindProperties]
    [ValidateAntiForgeryToken]
    public class LoginModel : PageModel
    {
        #region Services
        private readonly IUserService _userService;
        private readonly IUserRoleService _userRoleService;
        private readonly IUserTokenService _userToken;
        private readonly IConfiguration _configuration;
        private readonly IGoogleService _googleService;
        public LoginModel(IUserService userService, IUserRoleService userRoleService, IConfiguration configuration, IUserTokenService userToken, IGoogleService googleService)
        {
            _userService = userService;
            _userRoleService = userRoleService;
            _configuration = configuration;
            _userToken = userToken;
            _googleService = googleService;
        }
        #endregion

        #region Models

        [Display(Name = " شماره تماس")]
        [Required(ErrorMessage = "وارد کردن {0} اجباری است")]
        [MaxLength(11)]
        [MinLength(11)]
        public string PhoneNumber { get; set; }

        [Display(Name = " رمز عبور")]
        [Required(ErrorMessage = "وارد کردن {0} اجباری است")]
        [MinLength(6, ErrorMessage = "حداقل تعداد حروف بایستی 6 حرف باشد")]
        public string Password { get; set; }

        #endregion


        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPost()
        {


            if (!await _googleService.IsSatisfyAsync())
            {
                ModelState.AddModelError(nameof(PhoneNumber), "اعتبار سنجی recaptcha انجام نشد");
                return Page();
            }

            var user = _userService.Login(new LoginDto()
            {
                Password = Password,
                PhoneNumber = PhoneNumber
            });

            if(user == null)
            {
                ModelState.AddModelError(nameof(PhoneNumber), "کاربری یافت نشد");
                return Page();
            }
            Dictionary<string, bool> resultRole = new Dictionary<string, bool>();
            List<string> roles = new List<string>();

            var role = _userRoleService.GetRole(user.UserRoleId);
            resultRole.Add(nameof(role.Admin), role.Admin);
            resultRole.Add(nameof(role.User), role.User);
            resultRole.Add(nameof(role.Writer), role.Writer);
            resultRole.Add(nameof(role.Comment), role.Comment);
            resultRole.Add(nameof(role.Downloader), role.Downloader);


            foreach (var item in resultRole)
            {
                if(item.Value == true) 
                {
                    roles.Add(item.Key);
                }
            }

            string jwtToken = BuildJwtToken.BuildToken(roles, user, _configuration,null);
            
            var guid = Guid.NewGuid().ToString();

            string refreshJwtToken = BuildJwtToken.BuildToken(roles, user, _configuration, guid);

            HttpContext.Response.Cookies.Append("token", jwtToken, new CookieOptions()
            {
                HttpOnly = true,
                Expires = DateTimeOffset.Now.AddDays(7)
            });

            HttpContext.Response.Cookies.Append("refreshToken", refreshJwtToken, new CookieOptions()
            {
                HttpOnly = true,
                Expires = DateTimeOffset.Now.AddDays(10)
            });

            var result = _userToken.CreateToken(new CreateUserTokenDto()
            {
                CreationDate = DateTime.Now,
                RefreshTokenExpireDate = DateTime.Now.AddDays(10),
                TokenExpireDate = DateTime.Now.AddDays(7),
                HashJwtToken = Sha256Hasher.Hash(jwtToken),
                HashRefreshToken = Sha256Hasher.Hash(refreshJwtToken),
                UserId = user.Id,
                Device = DeviceInfornation.Info(HttpContext.Request.Headers["user-agent"])
            });
            //will add sweet Alert info
            if(result.Status != OperationResultStatus.Success)
            {
                ModelState.AddModelError(nameof(PhoneNumber), result.Message);
                return Redirect("../Index");

            }

            return Redirect("../Index");
        }
    }
}
