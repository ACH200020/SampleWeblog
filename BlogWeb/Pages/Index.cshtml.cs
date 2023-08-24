using BlogWeb.Infrastuctures.JwtBuilder;
using BlogWeb.Utilities;
using CoreLayer.DTOs.Post;
using CoreLayer.DTOs.UserToken;
using CoreLayer.Services.Post;
using CoreLayer.Services.User;
using CoreLayer.Services.UserToken;
using CoreLayer.Utilities;
using CoreLayer.Utilities.SecurityUtil;
using DataLayer.Entities.Users;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data;
using System.Runtime.Intrinsics.Arm;
using System.Security.Claims;

namespace BlogWeb.Pages
{
    
    public class IndexModel : PageModel
    {
        private readonly IPostService _postService;
        private readonly IUserTokenService _userToken;
        private readonly IUserService _userService;
        private readonly IConfiguration _configuration;
        public IndexModel(IPostService postService, IUserTokenService userToken, IUserService userService, IConfiguration configuration)
        {
            _postService = postService;
            _userToken = userToken;
            _userService = userService;
            _configuration = configuration;
        }

        public List<PostDto> Posts{ get; set; }
        public IActionResult OnGet()
        {


            var token = Request.Cookies["token"];
            var refreshToken = Request.Cookies["refreshToken"];

            if (token != null && refreshToken != null)
            {
                var user = _userService.GetUserById(User.GetUserId());
                var result = _userToken.CheckExpireToken(token, refreshToken, user.Id);

                if (result.Status == OperationResultStatus.NotFound)
                {
                    HttpContext.Response.Cookies.Delete("token");
                    HttpContext.Response.Cookies.Delete("refreshToken");
                }

                else if (result.Status == OperationResultStatus.Success)
                {
                    HttpContext.Response.Cookies.Delete("token");
                    HttpContext.Response.Cookies.Delete("refreshToken");

                    string jwtToken = BuildJwtToken.BuildToken(User.GetRole(), user, _configuration, null);

                    var guid = Guid.NewGuid().ToString();

                    string refreshJwtToken = BuildJwtToken.BuildToken(User.GetRole(), user, _configuration, guid);

                    _userToken.CreateToken(new CreateUserTokenDto()
                    {
                        CreationDate = DateTime.Now,
                        Device = DeviceInfornation.Info(HttpContext.Request.Headers["user-agent"]),
                        HashJwtToken = Sha256Hasher.Hash(jwtToken),
                        HashRefreshToken = Sha256Hasher.Hash(refreshJwtToken),
                        RefreshTokenExpireDate = DateTime.Now.AddDays(10),
                        TokenExpireDate = DateTime.Now.AddDays(7),
                        UserId = user.Id
                    });

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
                }
            }
            

            Posts = _postService.GetAllPosts();
            return Page();
        }
    }
}