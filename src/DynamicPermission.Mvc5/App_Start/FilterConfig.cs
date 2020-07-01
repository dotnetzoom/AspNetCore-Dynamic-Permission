using DynamicPermission.Mvc5.App_Code;
using System.Web.Mvc;

namespace DynamicPermission.Mvc5
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
            filters.Add(new PermissionAuthorizeAttribute());
        }
    }
}
