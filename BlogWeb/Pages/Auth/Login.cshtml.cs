using CoreLayer.DTOs.User;
using CoreLayer.Services.User;
using CoreLayer.Services.UserRole;
using CoreLayer.Utilities;
using DataLayer.Entities.Users;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.IdentityModel.Tokens;
using System.ComponentModel.DataAnnotations;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace BlogWeb.Pages.Auth
{
    [BindProperties]
    [ValidateAntiForgeryToken]
    public class LoginModel : PageModel
    {
        #region Services
        private readonly IUserService _userService;
        private readonly IUserRoleService _userRoleService;
        private readonly IConfiguration _configuration;
        public LoginModel(IUserService userService, IUserRoleService userRoleService, IConfiguration configuration)
        {
            _userService = userService;
            _userRoleService = userRoleService;
            _configuration = configuration;
        }
        #endregion

        #region Models

        [Display(Name = " شماره تماس")]
        [Required(ErrorMessage = "وارد کردن {0} اجباری است")]
        public string PhoneNumber { get; set; }

        [Display(Name = " رمز عبور")]
        [Required(ErrorMessage = "وارد کردن {0} اجباری است")]
        public string Password { get; set; }

        #endregion


        public void OnGet()
        {
        }

        public IActionResult OnPost()
        {
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

            var claims = new List<Claim>()
            {
                new Claim(ClaimTypes.MobilePhone,user.PhoneNumber),
                new Claim(ClaimTypes.NameIdentifier,user.Id.ToString()),

            };

            for (int i = 0; i < roles.Count; i++)
            {
                claims.Add(new Claim(ClaimTypes.Role, roles[i]));
            }

            var secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JwtConfig:SignInKey"]));
            var credential = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _configuration["JwtConfig:Issuer"],
                audience: _configuration["JwtConfig:Audience"],
                claims: claims,
                expires: DateTime.Now.AddDays(7),
                signingCredentials: credential);

            var jwtToken = new JwtSecurityTokenHandler().WriteToken(token);
            HttpContext.Response.Cookies.Append("token", jwtToken, new CookieOptions()
            {
                HttpOnly = true,
                Expires = DateTimeOffset.Now.AddDays(7)
            });
            return Redirect("../Index");
        }
    }
}
