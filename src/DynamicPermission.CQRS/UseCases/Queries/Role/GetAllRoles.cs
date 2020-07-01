using AutoMapper;
using AutoMapper.QueryableExtensions;
using DynamicPermission.CQRS.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Threading;
using System.Threading.Tasks;

namespace DynamicPermission.CQRS.UseCases
{
    public class GetAllRoles
    {
        public class Query : IRequest<List<ViewModel>>
        {
        }

        public class Mapping : Profile
        {
            public Mapping()
            {
                CreateMap<Role, ViewModel>();
            }
        }

        public class ViewModel
        {
            [Display(Name = "ردیف")]
            public int Id { get; set; }

            [Display(Name = "نام")]
            public string Name { get; set; }
        }

        public class Handler : IRequestHandler<Query, List<ViewModel>>
        {
            private readonly AppDbContext _dbContext;
            private readonly IMapper _mapper;

            public Handler(AppDbContext dbContext, IMapper mapper)
            {
                _dbContext = dbContext;
                _mapper = mapper;
            }

            public Task<List<ViewModel>> Handle(Query request, CancellationToken cancellationToken)
            {
                return _dbContext.Users
                    .ProjectTo<ViewModel>(_mapper.ConfigurationProvider)
                    .ToListAsync();
            }
        }
    }
}
