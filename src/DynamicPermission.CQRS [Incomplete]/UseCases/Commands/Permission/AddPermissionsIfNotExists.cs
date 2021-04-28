using DynamicPermission.CQRS.AppCode;
using DynamicPermission.CQRS.Models;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace DynamicPermission.CQRS.UseCases
{
    public class AddPermissionsIfNotExists
    {
        public class Command : IRequest
        {
            public int RoleId { get; set; }
            public List<string> ActionFullNames { get; set; }
        }

        public class Validator : AbstractValidator<Command>
        {
            public Validator()
            {
                RuleFor(v => v.ActionFullNames)
                    .NotNull()
                    .NotEmpty()
                    .WithMessage("نباید خالی باشد");
            }
        }

        [HandlerInfo("افزودن", "پرمیژن ها", "سطوح دسترسی")]
        public class Handler : IRequestHandler<Command>
        {
            private readonly AppDbContext _dbContext;

            public Handler(AppDbContext dbContext)
            {
                _dbContext = dbContext;
            }

            public async Task<Unit> Handle(Command request, CancellationToken cancellationToken)
            {
                foreach (var action in request.ActionFullNames)
                {
                    var permission = new Permission
                    {
                        RoleId = request.RoleId,
                        ActionFullName = action,
                    };
                    var exists = await _dbContext.Permissions.AnyAsync(p => p.RoleId == permission.RoleId && p.ActionFullName == permission.ActionFullName);
                    if (!exists)
                    {
                        _dbContext.Permissions.Add(permission);
                        await _dbContext.SaveChangesAsync();
                    }
                }
                return Unit.Value;
            }
        }
    }
}
