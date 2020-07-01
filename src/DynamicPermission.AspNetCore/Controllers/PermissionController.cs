using DynamicPermission.AspNetCore.App_Code;
using DynamicPermission.AspNetCore.Services;
using DynamicPermission.AspNetCore.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace DynamicPermission.AspNetCore.Controllers
{
    [ControllerInfo("پرمیژن ها", "احراز هویت")]
    public class PermissionController : Controller
    {
        private readonly IRoleService _roleService;
        private readonly IPermissionService _permissionService;

        public PermissionController(IRoleService roleService, IPermissionService permissionService)
        {
            _roleService = roleService;
            _permissionService = permissionService;
        }

        [HttpGet]
        public async Task<ActionResult> Index(int? id)
        {
            var allRoles = await _roleService.GetAllAsync();
            var model = new PermissionListViewModel { Roles = allRoles };

            var role = await _roleService.GetByIdIncludePermissionsAsync(id ?? 0);
            if (role != null)
            {
                model.RoleId = role.Id;
                model.Tabs = PermissionHelper.GetPermissionTabViewModels(role.Permissions);
            }

            return View(model);
        }

        [HttpPost]
        [ActionInfo("تغییر سطوح دسترسی")]
        public async Task<ActionResult> AddPermission(RolePermissionViewModel model)
        {
            await _permissionService.AddPermissionsIfNotExistsAsync(model);
            return Json(new { result = true });
        }

        [HttpPost]
        [ActionInfo("تغییر سطوح دسترسی")]
        public async Task<ActionResult> DeletePermission(RolePermissionViewModel model)
        {
            await _permissionService.DeletePermissionsAsync(model);
            return Json(new { result = true });
        }
    }
}