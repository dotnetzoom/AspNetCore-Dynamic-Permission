using DynamicPermission.Mvc5.Services;
using DynamicPermission.Mvc5.ViewModels;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.Web.Security;

namespace DynamicPermission.Mvc5.Controllers
{
    [AllowAnonymous]
    public class HomeController : Controller
    {
        private readonly IUserService _userService;

        public HomeController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet]
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public ActionResult Login()
        {
            var model = new UserLoginViewModel
            {
                UserName = "admin",
                Password = "admin"
            };
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Login(UserLoginViewModel userLoginViewModel)
        {
            if (ModelState.IsValid)
            {
                if (await _userService.ValidateLoginAsync(userLoginViewModel))
                {
                    FormsAuthentication.SetAuthCookie(userLoginViewModel.UserName, true);
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    ModelState.AddModelError("", "نام کاربری یا رمزعبور اشتباه است");
                }
            }
            return View(nameof(Index), userLoginViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public ActionResult AccessDenied()
        {
            return View();
        }
    }
}