using DynamicPermission.Mvc5.Services;
using System.Linq;
using System.Web.Mvc;

namespace DynamicPermission.Mvc5.App_Code
{
    public class PermissionAuthorizeAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (SkipAuthorization(filterContext.ActionDescriptor))
                return;

            if (!filterContext.HttpContext.User.Identity.IsAuthenticated)
            {
                filterContext.Result = new RedirectResult("/Home/AccessDenied");
                return;
            }

            var action = filterContext.ActionDescriptor.ActionName;
            var controller = filterContext.ActionDescriptor.ControllerDescriptor.ControllerType.FullName;
            var actionFullName = controller + "." + action;

            var permissionService = DependencyResolver.Current.GetService<IPermissionService>();
            var hasPermission = permissionService.UserHasPermissionAsync(filterContext.HttpContext.User.Identity.Name, actionFullName).GetAwaiter().GetResult();
            if (!hasPermission)
                filterContext.Result = new RedirectResult("/Home/AccessDenied");
        }

        private static bool SkipAuthorization(ActionDescriptor actionContext)
        {
            return actionContext.GetCustomAttributes(typeof(AllowAnonymousAttribute), true).Any()
                       || actionContext.ControllerDescriptor.GetCustomAttributes(typeof(AllowAnonymousAttribute), true).Any();
        }
    }
}