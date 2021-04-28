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
    public class GetAllUsers
    {
        public class Query : IRequest<List<ViewModel>>
        {
        }

        public class Mapping : Profile
        {
            public Mapping()
            {
                CreateMap<User, ViewModel>();
            }
        }

        public class ViewModel
        {
            [Display(Name = "ردیف")]
            public int Id { get; set; }

            [Display(Name = "نام و نام خانوادگی")]
            public string FullName { get; set; }

            [Display(Name = "نام کاربری")]
            public string UserName { get; set; }

            [Display(Name = "رمز عبور")]
            public string Password { get; set; }

            [Display(Name = "نقش ها")]
            public string Roles { get; set; }
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
