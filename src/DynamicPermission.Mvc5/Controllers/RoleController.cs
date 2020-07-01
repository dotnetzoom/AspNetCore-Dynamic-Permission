using AutoMapper;
using DynamicPermission.Mvc5.App_Code;
using DynamicPermission.Mvc5.Services;
using DynamicPermission.Mvc5.ViewModels;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace DynamicPermission.Mvc5.Controllers
{
    [ControllerInfo("نقش ها", "احراز هویت")]
    public class RoleController : Controller
    {
        private readonly IRoleService _roleService;
        private readonly IMapper _mapper;

        public RoleController(IRoleService roleService, IMapper mapper)
        {
            _roleService = roleService;
            _mapper = mapper;
        }

        public async Task<ActionResult> Index()
        {
            var roles = await _roleService.GetAllAsync();
            var list = _mapper.Map<List<RoleViewModel>>(roles);
            return View(list);
        }

        [HttpGet]
        public ActionResult Create()
        {
            var roleViewModel = new RoleViewModel();
            return View(roleViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(RoleViewModel roleViewModel)
        {
            if (ModelState.IsValid)
            {
                await _roleService.AddAsync(roleViewModel);
                return RedirectToAction(nameof(Index));
            }
            return View(roleViewModel);
        }

        [HttpGet]
        public async Task<ActionResult> Edit(int id)
        {
            var role = await _roleService.GetByIdAsync(id);
            var roleViewModel = _mapper.Map<RoleViewModel>(role);
            return View(roleViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(RoleViewModel roleViewModel)
        {
            if (ModelState.IsValid)
            {
                await _roleService.UpdateAsync(roleViewModel);
                return RedirectToAction(nameof(Index));
            }
            return View(roleViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Delete(int id)
        {
            await _roleService.DeleteAsync(id);
            return RedirectToAction(nameof(Index));
        }
    }
}