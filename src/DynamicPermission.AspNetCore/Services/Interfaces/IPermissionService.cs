using DynamicPermission.AspNetCore.Models;
using DynamicPermission.AspNetCore.ViewModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DynamicPermission.AspNetCore.Services
{
    public interface IPermissionService
    {
        Task AddPermissionsIfNotExistsAsync(RolePermissionViewModel model);
        Task DeletePermissionsAsync(RolePermissionViewModel model);
        Task AddRangeAsync(IEnumerable<Permission> permissions);
        Task<bool> UserHasPermissionAsync(string username, string actionFullName);
    }
}