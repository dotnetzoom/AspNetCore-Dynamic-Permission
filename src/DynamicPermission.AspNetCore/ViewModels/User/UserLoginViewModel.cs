using System.ComponentModel.DataAnnotations;

namespace DynamicPermission.AspNetCore.ViewModels
{
    public class UserLoginViewModel
    {
        [Display(Name = "نام کاربری")]
        [Required(ErrorMessage = "{0} نباید خالی باشد")]
        [StringLength(50, ErrorMessage = "{0} نباید بیشتر از 50 کاراکتر باشد")]
        public string UserName { get; set; }

        [Display(Name = "رمز عبور")]
        [Required(ErrorMessage = "{0} نباید خالی باشد")]
        public string Password { get; set; }
    }
}