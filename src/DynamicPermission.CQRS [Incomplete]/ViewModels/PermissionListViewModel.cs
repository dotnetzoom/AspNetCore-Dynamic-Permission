using DynamicPermission.CQRS.Models;
using DynamicPermission.CQRS.UseCases;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;

namespace DynamicPermission.CQRS.ViewModels
{
    public class PermissionListViewModel
    {
        public int? RoleId { get; set; }
        public List<GetAllRoles.ViewModel> Roles { get; set; }
        public List<PermissionTab> PermissionTabs { get; set; } = new List<PermissionTab>();

        public SelectList GetRolesSelectList()
        {
            return new SelectList(Roles, nameof(GetAllRoles.ViewModel.Id), nameof(GetAllRoles.ViewModel.Name));
        }
    }
}
