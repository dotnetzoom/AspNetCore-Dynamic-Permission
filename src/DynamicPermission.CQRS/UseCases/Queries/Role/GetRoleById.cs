using AutoMapper;
using AutoMapper.QueryableExtensions;
using DynamicPermission.CQRS.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;

namespace DynamicPermission.CQRS.UseCases
{
    public class GetRoleById
    {
        public class Query : IRequest<UpdateRole.Command>
        {
            public int Id { get; set; }
        }

        public class Handler : IRequestHandler<Query, UpdateRole.Command>
        {
            private readonly AppDbContext _dbContext;
            private readonly IMapper _mapper;

            public Handler(AppDbContext dbContext, IMapper mapper)
            {
                _dbContext = dbContext;
                _mapper = mapper;
            }

            public Task<UpdateRole.Command> Handle(Query request, CancellationToken cancellationToken)
            {
                return _dbContext.Users
                    .ProjectTo<UpdateRole.Command>(_mapper.ConfigurationProvider)
                    .FirstOrDefaultAsync(p => p.Id == request.Id);
            }
        }
    }
}
