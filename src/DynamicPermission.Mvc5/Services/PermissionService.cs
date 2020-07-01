using DynamicPermission.Mvc5.Models;
using DynamicPermission.Mvc5.ViewModels;
using EFSecondLevelCache;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

namespace DynamicPermission.Mvc5.Services
{
    public class PermissionService : IPermissionService
    {
        private readonly AppDbContext _dbContext;

        public PermissionService(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task AddPermissionsIfNotExistsAsync(RolePermissionViewModel model)
        {
            foreach (var action in model.ActionFullNames)
            {
                var permission = new Permission
                {
                    RoleId = model.RoleId,
                    ActionFullName = action,
                };
                var exists = await _dbContext.Permissions.AnyAsync(p => p.RoleId == permission.RoleId && p.ActionFullName == permission.ActionFullName);
                if (!exists)
                {
                    _dbContext.Permissions.Add(permission);
                    await _dbContext.SaveChangesAsync();
                }
            }
        }

        public async Task DeletePermissionsAsync(RolePermissionViewModel model)
        {
            var permissions = await _dbContext.Permissions.Where(p => p.RoleId == model.RoleId && model.ActionFullNames.Contains(p.ActionFullName)).ToListAsync();
            _dbContext.Permissions.RemoveRange(permissions);
            await _dbContext.SaveChangesAsync();
        }

        public Task AddRangeAsync(IEnumerable<Permission> permissions)
        {
            _dbContext.Permissions.AddRange(permissions);
            return _dbContext.SaveChangesAsync();
        }

        public async Task<bool> UserHasPermissionAsync(string username, string actionFullName)
        {
            var userRolesId = await _dbContext.UserRoles.Where(p => p.User.UserName == username).Cacheable().Select(p => p.RoleId).ToListAsync();
            var permissions = await _dbContext.Permissions.Cacheable().Select(p => new { p.RoleId, p.ActionFullName }).ToListAsync();

            var hasPermission = permissions.Any(p => userRolesId.Contains(p.RoleId) &&
                 p.ActionFullName.Equals(actionFullName, StringComparison.OrdinalIgnoreCase));

            return hasPermission;
        }
    }
}