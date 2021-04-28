using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Linq;
using System.Threading.Tasks;

namespace DynamicPermission.CQRS.App_Code
{
    public class PermissionAuthorizeAttribute : ActionFilterAttribute
    {
        private readonly IPermissionService _permissionService;

        public PermissionAuthorizeAttribute(IPermissionService permissionService)
        {
            _permissionService = permissionService;
        }

        public override async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var actionDescriptor = (ControllerActionDescriptor)context.ActionDescriptor;
            if (SkipAuthorization(actionDescriptor))
            {
                await base.OnActionExecutionAsync(context, next);
                return;
            }

            if (!context.HttpContext.User.Identity.IsAuthenticated)
            {
                context.Result = new RedirectResult("/Home/AccessDenied");
                return;
            }

            var action = actionDescriptor.ActionName;
            var controller = actionDescriptor.ControllerTypeInfo.FullName;
            var actionFullName = controller + "." + action;

            var hasPermission = await _permissionService.UserHasPermissionAsync(context.HttpContext.User.Identity.Name, actionFullName);
            if (!hasPermission)
            {
                context.Result = new RedirectResult("/Home/AccessDenied");
                return;
            }

            await base.OnActionExecutionAsync(context, next);
        }

        private static bool SkipAuthorization(ControllerActionDescriptor actionDescriptor)
        {
            return actionDescriptor.EndpointMetadata.OfType<AllowAnonymousAttribute>().Any();
        }
    }
}