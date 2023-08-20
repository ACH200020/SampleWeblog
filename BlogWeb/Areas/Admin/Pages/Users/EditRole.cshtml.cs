using CoreLayer.DTOs.User;
using CoreLayer.DTOs.UserRole;
using CoreLayer.Services.User;
using CoreLayer.Services.UserRole;
using CoreLayer.Utilities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;

namespace BlogWeb.Areas.Admin.Pages.Users
{
    [ValidateAntiForgeryToken]
    [BindProperties]
    public class EditRoleModel : PageModel
    {
        #region Services
        private readonly IUserService _userService;
        private readonly IUserRoleService _roleServce;

        public EditRoleModel(IUserRoleService roleServce, IUserService userService)
        {
            _roleServce = roleServce;
            _userService = userService;
        }
        #endregion

        #region Models
        [Display(Name = "فعال بودن کاربر")]
        public bool IsActive { get; set; }

        [Display(Name = "ادمین")]
        public bool Admin { get; set; }

        [Display(Name = "کاربر")]
        public bool User { get; set; }

        [Display(Name = "نویسنده")]
        public bool Writer { get; set; }

        [Display(Name = "کامنت گذاشتن")]
        public bool Comment { get; set; }

        [Display(Name = "دانلود کردن")]
        public bool Downloader { get; set; }
        #endregion

        public void OnGet(int id)
        {
            var user = _userService.GetUserById(id);
            IsActive = user.IsActive;

            var role = _roleServce.GetRole(user.UserRoleId);
            Admin = role.Admin;
            Writer = role.Writer;
            Comment = role.Comment;
            Downloader = role.Downloader;
            User = role.User;

            
        }

        public IActionResult OnPost(int id)
        {
            var user = _userService.GetUserById(id);

            var result = _userService.EditUser(new EditUserDto()
            {
                Id = id,
                IsActive = IsActive
            });

            result = _roleServce.EditUserRole(new EditUserRoleDto()
            {
                Id = user.UserRoleId,
                Admin = Admin,
                Writer = Writer,
                Comment = Comment,
                Downloader = Downloader,
                User = User
            });

            if(result.Status != OperationResultStatus.Success)
            {
                ModelState.AddModelError(nameof(IsActive), result.Message);
                return Page();
            }

            return RedirectToPage("Index");

        }
    }
}
