using DynamicPermission.Mvc5.Models;
using DynamicPermission.Mvc5.ViewModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DynamicPermission.Mvc5.Services
{
    public interface IPermissionService
    {
        Task AddPermissionsIfNotExistsAsync(RolePermissionViewModel model);
        Task DeletePermissionsAsync(RolePermissionViewModel model);
        Task AddRangeAsync(IEnumerable<Permission> permissions);
        Task<bool> UserHasPermissionAsync(string username, string actionFullName);
    }
}