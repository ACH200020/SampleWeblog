using CoreLayer.DTOs.User;
using CoreLayer.Services.User;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BlogWeb.Areas.Admin.Pages.Users
{
    public class IndexModel : PageModel
    {
        #region Services

        private readonly IUserService _userService;

        public IndexModel(IUserService userService)
        {
            _userService = userService;
        }
        #endregion

        #region Models
        public List<UserDto> Users { get; set; }
        #endregion
        public IActionResult OnGet()
        {
            Users = _userService.GetUsers();
            return Page();
        }
    }
}
