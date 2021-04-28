using DynamicPermission.CQRS.Models;
using EFCoreSecondLevelCacheInterceptor;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace DynamicPermission.CQRS.UseCases
{
    public class UserHasPermission
    {
        public class Query : IRequest<bool>
        {
            public string UserName { get; set; }

            public string ActionFullName { get; set; }
        }

        public class Validator : AbstractValidator<Query>
        {
            public Validator()
            {
                RuleFor(v => v.UserName)
                    .NotNull()
                    .NotEmpty()
                    .WithMessage("{PropertyName} نباید خالی باشد");

                RuleFor(v => v.ActionFullName)
                    .NotNull()
                    .NotEmpty()
                    .WithMessage("{PropertyName} نباید خالی باشد");
            }
        }

        public class Handler : IRequestHandler<Query, bool>
        {
            private readonly AppDbContext _dbContext;

            public Handler(AppDbContext dbContext)
            {
                _dbContext = dbContext;
            }

            public async Task<bool> Handle(Query request, CancellationToken cancellationToken)
            {
                var userRolesId = await _dbContext.UserRoles.Where(p => p.User.UserName == request.UserName).Cacheable().Select(p => p.RoleId).ToListAsync();
                var permissions = await _dbContext.Permissions.Cacheable().Select(p => new { p.RoleId, p.ActionFullName }).ToListAsync();

                var hasPermission = permissions.Any(p => userRolesId.Contains(p.RoleId) &&
                     p.ActionFullName.Equals(request.ActionFullName, StringComparison.OrdinalIgnoreCase));

                return hasPermission;
            }
        }
    }
}
