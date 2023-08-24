using CoreLayer.DTOs.User;
using CoreLayer.DTOs.UserRole;
using CoreLayer.Services.User;
using CoreLayer.Services.UserRole;
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
        private readonly IUserRoleService _userRoleService;
        public RegisterModel(IUserService userService, IUserRoleService userRoleService)
        {
            _userService = userService;
            _userRoleService = userRoleService;

        }

       

        #endregion

        #region Models
        [Display(Name = " نام")]
        [Required(ErrorMessage = "وارد کردن {0} اجباری است")]
        public string Name { get; set; }

        [Display(Name = "نام خانوادگی")]
        [Required(ErrorMessage = "وارد کردن {0} اجباری است")]
        public string Family { get; set; }
        
        [Display(Name = "کدملی")]
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
        [MinLength(6,ErrorMessage = "حداقل تعداد حروف بایستی 6 حرف باشد")]
        public string Password { get; set; }
        #endregion

        public void OnGet()
        {
        }

        public IActionResult OnPost()
        {

            int userRoleId = _userRoleService.CreateUserRole(new CreateUserRoleDto()
            {
                Admin = false,
                Comment = true,
                Downloader = true,
                User = true,
                Writer = false
            });

            var result = _userService.RegisterUser(new RegisterUserDto()
            {
                CreationDate = DateTime.Now,
                Name = Name,
                Family = Family,
                NationalCode = NationalCode,
                PhoneNumber = PhoneNumber,
                Password = Password,
                UserRoleId = userRoleId
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
