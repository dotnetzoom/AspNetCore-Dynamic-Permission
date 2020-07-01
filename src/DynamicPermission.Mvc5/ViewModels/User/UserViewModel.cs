using DynamicPermission.Mvc5.Models;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web.Mvc;

namespace DynamicPermission.Mvc5.ViewModels
{
    public class UserViewModel
    {
        [Display(Name = "ردیف")]
        public int Id { get; set; }

        [Display(Name = "نام و نام خانوادگی")]
        [Required(ErrorMessage = "{0} نباید خالی باشد")]
        [StringLength(50, ErrorMessage = "{0} نباید بیشتر از 50 کاراکتر باشد")]
        public string FullName { get; set; }

        [Display(Name = "نام کاربری")]
        [Required(ErrorMessage = "{0} نباید خالی باشد")]
        [StringLength(50, ErrorMessage = "{0} نباید بیشتر از 50 کاراکتر باشد")]
        public string UserName { get; set; }

        [Display(Name = "رمز عبور")]
        [Required(ErrorMessage = "{0} نباید خالی باشد")]
        public string Password { get; set; }

        [Display(Name = "نقش ها")]
        public List<int> SelectedRoles { get; set; }

        [Display(Name = "نقش ها")]
        public string Roles { get; set; }

        public List<Role> AllRoles { get; set; }

        public SelectList GetRolesSelectList()
        {
            return new SelectList(AllRoles, nameof(Role.Id), nameof(Role.Name));
        }

        public class Profile : AutoMapper.Profile
        {
            public Profile()
            {
                CreateMap<UserViewModel, User>()
                    .ReverseMap()
                    .ForMember(p => p.SelectedRoles, p => p.MapFrom(q => q.UserRoles.Select(x => x.RoleId).ToList()))
                    .ForMember(p => p.Roles, p => p.MapFrom(q => string.Join(", ", q.UserRoles.Select(x => x.Role.Name))));
            }
        }
    }
}