using DynamicPermission.CQRS.UseCases;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace DynamicPermission.CQRS.Controllers
{
    public class UserController : Controller
    {
        private readonly IMediator _mediator;

        public UserController(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task<ActionResult> Index()
        {
            var list = await _mediator.Send(new GetAllUsers.Query());
            return View(list);
        }

        [HttpGet]
        public async Task<ActionResult> Create()
        {
            var command = new CreateUser.Command();
            await PrepareViewModelAsync(command);
            return View(command);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(CreateUser.Command command)
        {
            if (ModelState.IsValid)
            {
                await _mediator.Send(command);
                return RedirectToAction(nameof(Index));
            }
            await PrepareViewModelAsync(command);
            return View(command);
        }

        [HttpGet]
        public async Task<ActionResult> Edit(GetUserById.Query query)
        {
            var viewModel = await _mediator.Send(query);
            await PrepareViewModelAsync(viewModel);
            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(UpdateUser.Command command)
        {
            if (ModelState.IsValid)
            {
                await _mediator.Send(command);
                return RedirectToAction(nameof(Index));
            }
            await PrepareViewModelAsync(command);
            return View(command);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Delete(DeleteRole.Command command)
        {
            await _mediator.Send(command);
            return RedirectToAction(nameof(Index));
        }

        private async Task PrepareViewModelAsync(CreateUser.Command command)
        {
            var roles = await _mediator.Send(new GetAllRoles.Query());
            command.AllRoles = roles;
        }
    }
}