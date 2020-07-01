using DynamicPermission.Mvc5.Models;
using System.ComponentModel.DataAnnotations;

namespace DynamicPermission.Mvc5.ViewModels
{
    public class RoleViewModel
    {
        [Display(Name = "ردیف")]
        public int Id { get; set; }

        [Display(Name = "نام")]
        [Required(ErrorMessage = "{0} نباید خالی باشد")]
        [StringLength(50, ErrorMessage = "{0} نباید بیشتر از 50 کاراکتر باشد")]
        public string Name{ get; set; }

        public class Profile : AutoMapper.Profile
        {
            public Profile()
            {
                CreateMap<RoleViewModel, Role>().ReverseMap();
            }
        }
    }
}