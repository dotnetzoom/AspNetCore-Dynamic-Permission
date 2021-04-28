using AutoMapper;
using DynamicPermission.CQRS.UseCases;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DynamicPermission.CQRS.Controllers
{
    public class RoleController : Controller
    {
        private readonly IMediator _mediator;

        public RoleController(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task<ActionResult> Index()
        {
            var roles = await _mediator.Send(new GetAllRoles.Query());
            return View(roles);
        }

        [HttpGet]
        public ActionResult Create()
        {
            var command = new CreateRole.Command();
            return View(command);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(CreateRole.Command command)
        {
            if (ModelState.IsValid)
            {
                await _mediator.Send(command);
                return RedirectToAction(nameof(Index));
            }
            return View(command);
        }

        [HttpGet]
        public async Task<ActionResult> Edit(GetRoleById.Query query)
        {
            var command = await _mediator.Send(query);
            return View(command);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(UpdateRole.Command command)
        {
            if (ModelState.IsValid)
            {
                await _mediator.Send(command);
                return RedirectToAction(nameof(Index));
            }
            return View(command);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Delete(DeleteRole.Command command)
        {
            await _mediator.Send(command);
            return RedirectToAction(nameof(Index));
        }
    }
}