using DynamicPermission.CQRS.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;

namespace DynamicPermission.CQRS.UseCases
{
    public class GetRoleByIdIncludePermissions
    {
        public class Query : IRequest<Role>
        {
            public int Id { get; set; }
        }

        public class Handler : IRequestHandler<Query, Role>
        {
            private readonly AppDbContext _dbContext;

            public Handler(AppDbContext dbContext)
            {
                _dbContext = dbContext;
            }

            public Task<Role> Handle(Query request, CancellationToken cancellationToken)
            {
                return _dbContext.Roles.Include(p => p.Permissions).FirstOrDefaultAsync(p => p.Id == request.Id);
            }
        }
    }
}
