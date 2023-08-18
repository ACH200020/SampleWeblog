using CoreLayer.DTOs.User;
using CoreLayer.Services.User;
using CoreLayer.Utilities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;

namespace BlogWeb.Pages.Auth
{
    [BindProperties]
    [ValidateAntiForgeryToken]
    public class LoginModel : PageModel
    {
        #region Services
        private readonly IUserService _userService;

        public LoginModel(IUserService userService)
        {
            _userService = userService;
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
            var result = _userService.Login(new LoginDto()
            {
                Password = Password,
                PhoneNumber = PhoneNumber
            });

            if(result.Status != OperationResultStatus.Success)
            {
                ModelState.AddModelError(nameof(PhoneNumber), "کاربری یافت نشد");
                return Page();
            }

            return Redirect("Index");
        }
    }
}
