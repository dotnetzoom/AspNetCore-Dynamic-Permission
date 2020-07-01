using AutoMapper;
using AutoMapper.QueryableExtensions;
using DynamicPermission.CQRS.AppCode;
using DynamicPermission.CQRS.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;

namespace DynamicPermission.CQRS.UseCases
{
    public class GetUserById
    {
        [HandlerInfo("افزودن", "کاربر ها", "مدیریت کاربران")]
        public class Query : IRequest<UpdateUser.Command>
        {
            public int Id { get; set; }
        }

        public class Handler : IRequestHandler<Query, UpdateUser.Command>
        {
            private readonly AppDbContext _dbContext;
            private readonly IMapper _mapper;

            public Handler(AppDbContext dbContext, IMapper mapper)
            {
                _dbContext = dbContext;
                _mapper = mapper;
            }

            public Task<UpdateUser.Command> Handle(Query request, CancellationToken cancellationToken)
            {
                return _dbContext.Users
                    .ProjectTo<UpdateUser.Command>(_mapper.ConfigurationProvider)
                    .FirstOrDefaultAsync(p => p.Id == request.Id);
            }
        }
    }
}
