using DynamicPermission.Mvc5.App_Code;
using DynamicPermission.Mvc5.Services;
using DynamicPermission.Mvc5.ViewModels;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace DynamicPermission.Mvc5.Controllers
{
    [ControllerInfo("کاربران")]
    public class UserController : Controller
    {
        private readonly IUserService _userService;
        private readonly IRoleService _roleService;
        public UserController(IUserService userService, IRoleService roleService)
        {
            _userService = userService;
            _roleService = roleService;
        }

        public async Task<ActionResult> Index()
        {
            var list = await _userService.GetAllWithRolesAsync();
            return View(list);
        }

        [HttpGet]
        public async Task<ActionResult> Create()
        {
            var userViewModel = new UserViewModel();
            await PrepareViewModelAsync(userViewModel);
            return View(userViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(UserViewModel userViewModel)
        {
            if (ModelState.IsValid)
            {
                await _userService.AddAsync(userViewModel);
                return RedirectToAction(nameof(Index));
            }
            await PrepareViewModelAsync(userViewModel);
            return View(userViewModel);
        }

        [HttpGet]
        public async Task<ActionResult> Edit(int id)
        {
            var userViewModel = await _userService.GetByIdWithRolesAsync(id);
            await PrepareViewModelAsync(userViewModel);
            return View(userViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(UserViewModel userViewModel)
        {
            if (ModelState.IsValid)
            {
                await _userService.UpdateAsync(userViewModel);
                return RedirectToAction(nameof(Index));
            }
            await PrepareViewModelAsync(userViewModel);
            return View(userViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Delete(int id)
        {
            await _userService.DeleteAsync(id);
            return RedirectToAction(nameof(Index));
        }

        private async Task PrepareViewModelAsync(UserViewModel userViewModel)
        {
            var roles = await _roleService.GetAllAsync();
            userViewModel.AllRoles = roles;
        }
    }
}