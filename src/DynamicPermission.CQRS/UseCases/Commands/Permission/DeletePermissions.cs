using DynamicPermission.CQRS.AppCode;
using DynamicPermission.CQRS.Models;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace DynamicPermission.CQRS.UseCases
{
    public class DeletePermissions
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

        [HandlerInfo("حذف", "پرمیژن ها", "سطوح دسترسی")]
        public class Handler : IRequestHandler<Command>
        {
            private readonly AppDbContext _dbContext;

            public Handler(AppDbContext dbContext)
            {
                _dbContext = dbContext;
            }

            public async Task<Unit> Handle(Command request, CancellationToken cancellationToken)
            {
                var permissions = await _dbContext.Permissions.Where(p => p.RoleId == request.RoleId && request.ActionFullNames.Contains(p.ActionFullName)).ToListAsync();
                _dbContext.Permissions.RemoveRange(permissions);
                await _dbContext.SaveChangesAsync();

                return Unit.Value;
            }
        }
    }
}
