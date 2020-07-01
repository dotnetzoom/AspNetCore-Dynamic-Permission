using DynamicPermission.Mvc5.Models;
using System.Collections.Generic;
using System.Web.Mvc;

namespace DynamicPermission.Mvc5.ViewModels
{
    public class PermissionListViewModel
    {
        public int? RoleId { get; set; }
        public List<Role> Roles { get; set; }
        public List<PermissionTab> Tabs { get; set; } = new List<PermissionTab>();

        public SelectList GetRolesSelectList()
        {
            return new SelectList(Roles, nameof(Role.Id), nameof(Role.Name));
        }
    }
}
