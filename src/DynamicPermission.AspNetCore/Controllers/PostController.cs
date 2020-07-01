using DynamicPermission.AspNetCore.App_Code;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DynamicPermission.AspNetCore.Controllers
{
    /// <summary>
    /// ============ Dummy Controller for Test ============
    /// </summary>
    [ControllerInfo("پست ها", "محتوا")]
    public class PostController : Controller
    {
        [ActionInfo("نمایش")]
        public ActionResult Index()
        {
            return Content("Index Action");
        }

        [ActionInfo("نمایش")]
        public ActionResult Details()
        {
            return Content("Index Action");
        }

        public ActionResult Create()
        {
            return Content("Create Action");
        }

        [ActionInfo("ویرایش")]
        public ActionResult Edit()
        {
            return Content("Edit Action");
        }

        [ActionInfo("ویرایش")]
        public ActionResult Update()
        {
            return Content("Edit Action");
        }

        public ActionResult Delete()
        {
            return Content("Delete Action");
        }

        [AllowAnonymous]
        public ActionResult AllowAnonymousActionMethodsNotIncluded()
        {
            return Content("Allow-Anonymous Action");
        }

        [NonAction]
        public ActionResult NonActionMethodsNotIncluded()
        {
            return Content("Non-Action Action");
        }

        private ActionResult NonPublicActionMethodsNotIncluded()
        {
            return Content("Non-Public Action");
        }
    }
}