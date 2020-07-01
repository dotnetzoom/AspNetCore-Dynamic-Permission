using DynamicPermission.CQRS.AppCode;
using DynamicPermission.CQRS.Models;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace DynamicPermission.CQRS.UseCases
{
    public class DeleteRole
    {
        [HandlerInfo("حذف", "نقش ها", "سطوح دسترسی")]
        public class Command : IRequest
        {
            public int Id { get; set; }
        }

        public class Handler : IRequestHandler<Command>
        {
            private readonly AppDbContext _dbContext;

            public Handler(AppDbContext dbContext)
            {
                _dbContext = dbContext;
            }

            public async Task<Unit> Handle(Command request, CancellationToken cancellationToken)
            {
                var role = await _dbContext.Roles.FindAsync(request.Id);
                _dbContext.Roles.Remove(role);
                await _dbContext.SaveChangesAsync();
                return Unit.Value;
            }
        }
    }
}
