using DynamicPermission.Mvc5.App_Code;
using DynamicPermission.Mvc5.Services;
using DynamicPermission.Mvc5.ViewModels;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace DynamicPermission.Mvc5.Controllers
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
            return Json(new { result = true }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [ActionInfo("تغییر سطوح دسترسی")]
        public async Task<ActionResult> DeletePermission(RolePermissionViewModel model)
        {
            await _permissionService.DeletePermissionsAsync(model);
            return Json(new { result = true }, JsonRequestBehavior.AllowGet);
        }
    }
}