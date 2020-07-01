using DynamicPermission.CQRS.AppCode;
using DynamicPermission.CQRS.Models;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace DynamicPermission.CQRS.UseCases
{
    public class DeleteUser
    {
        [HandlerInfo("حذف", "کاربر ها", "مدیریت کاربران")]
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
                var user = await _dbContext.Users.FindAsync(request.Id);
                _dbContext.Users.Remove(user);
                await _dbContext.SaveChangesAsync();
                return Unit.Value;
            }
        }
    }
}
