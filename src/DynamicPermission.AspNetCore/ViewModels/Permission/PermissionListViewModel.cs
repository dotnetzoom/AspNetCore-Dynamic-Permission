using DynamicPermission.AspNetCore.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;

namespace DynamicPermission.AspNetCore.ViewModels
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
