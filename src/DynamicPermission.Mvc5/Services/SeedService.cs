using DynamicPermission.Mvc5.App_Code;
using DynamicPermission.Mvc5.Models;
using DynamicPermission.Mvc5.ViewModels;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DynamicPermission.Mvc5.Services
{
    public class SeedService : ISeedService
    {
        private readonly IUserService _userService;
        private readonly IRoleService _roleService;
        private readonly IPermissionService _permissionService;

        public SeedService(IUserService userService, IRoleService roleService, IPermissionService permissionService)
        {
            _userService = userService;
            _roleService = roleService;
            _permissionService = permissionService;
        }

        public async Task SeedAsync()
        {
            await _roleService.AddAsync(new RoleViewModel
            {
                Name = "مدیر"
            });
            await _roleService.AddAsync(new RoleViewModel
            {
                Name = "نویسنده"
            });
            await _userService.AddAsync(new UserViewModel
            {
                FullName = "محمد جواد ابراهیمی",
                Password = "admin",
                UserName = "admin",
                SelectedRoles = new List<int> { 1 }
            });
            await _userService.AddAsync(new UserViewModel
            {
                FullName = "کاربر تست",
                Password = "writer",
                UserName = "writer",
                SelectedRoles = new List<int> { 2 }
            });

            var allActionFullNames = PermissionHelper.Tabs
                .SelectMany(tab => tab.Controllers.SelectMany(controller => controller.Actions.SelectMany(action => action.FullNames)))
                .Distinct().ToList();

            var permissions = allActionFullNames.Select(p => new Permission { ActionFullName = p, RoleId = 1 }).ToList();
            await _permissionService.AddRangeAsync(permissions);
        }
    }
}