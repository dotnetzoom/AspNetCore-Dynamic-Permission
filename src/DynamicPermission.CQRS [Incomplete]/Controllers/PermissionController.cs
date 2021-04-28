using DynamicPermission.CQRS.App_Code;
using DynamicPermission.CQRS.UseCases;
using DynamicPermission.CQRS.ViewModels;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace DynamicPermission.CQRS.Controllers
{
    public class PermissionController : Controller
    {
        private readonly IMediator _mediator;

        public PermissionController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<ActionResult> Index(int? id)
        {
            var allRoles = await _mediator.Send(new GetAllRoles.Query());
            var model = new PermissionListViewModel { Roles = allRoles };

            var role = await _mediator.Send(new GetRoleByIdIncludePermissions.Query { Id = id ?? 0 });
            if (role != null)
            {
                model.RoleId = role.Id;
                model.PermissionTabs = PermissionHelper.GetPermissionTabViewModels(role.Permissions);
            }

            return View(model);
        }

        [HttpPost]
        public async Task<ActionResult> AddPermission(AddPermissionsIfNotExists.Command command)
        {
            await _mediator.Send(command);
            return Json(new { result = true });
        }

        [HttpPost]
        public async Task<ActionResult> DeletePermission(DeletePermissions.Command command)
        {
            await _mediator.Send(command);
            return Json(new { result = true });
        }
    }
}