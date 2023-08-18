using CoreLayer.DTOs.User;
using CoreLayer.Services.User;
using CoreLayer.Utilities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;

namespace BlogWeb.Pages.Auth
{
    [ValidateAntiForgeryToken]
    [BindProperties]
    public class RegisterModel : PageModel
    {
        #region Services
        private readonly IUserService _userService;

        public RegisterModel(IUserService userService)
        {
            _userService = userService;
        }

        #endregion

        #region Models
        [Display(Name = " نام")]
        [Required(ErrorMessage = "وارد کردن {0} اجباری است")]
        public string Name { get; set; }

        [Display(Name = "نام خانوادگی")]
        [Required(ErrorMessage = "وارد کردن {0} اجباری است")]
        public string Family { get; set; }
        
        [Display(Name = " نام")]
        [Required(ErrorMessage = "وارد کردن {0} اجباری است")]
        [MaxLength(10)]
        [MinLength(10)]
        public string NationalCode { get; set; }

        [Display(Name = " شماره تماس")]
        [Required(ErrorMessage = "وارد کردن {0} اجباری است")]
        [MaxLength(11)]
        [MinLength(11)]
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
            var result = _userService.AddUser(new AddUserDto()
            {
                CreationDate = DateTime.Now,
                Name = Name,
                Family = Family,
                NationalCode = NationalCode,
                PhoneNumber = PhoneNumber,
                Password = Password
            });

            if(result.Status != OperationResultStatus.Success)
            {
                ModelState.AddModelError(nameof(PhoneNumber), result.Message);
                return Page();
            }

            return Redirect("Login");

        }
    }
}
