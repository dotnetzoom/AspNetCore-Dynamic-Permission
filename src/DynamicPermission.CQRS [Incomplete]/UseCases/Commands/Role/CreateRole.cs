using AutoMapper;
using DynamicPermission.CQRS.AppCode;
using DynamicPermission.CQRS.Models;
using FluentValidation;
using MediatR;
using System.ComponentModel.DataAnnotations;
using System.Threading;
using System.Threading.Tasks;

namespace DynamicPermission.CQRS.UseCases
{
    public class CreateRole
    {
        [HandlerInfo("افزودن", "نقش ها", "سطوح دسترسی")]
        public class Command : IRequest
        {
            [Display(Name = "نام")]
            public string Name { get; set; }
        }

        public class Mapping : Profile
        {
            public Mapping()
            {
                CreateMap<Command, Role>();
            }
        }

        public class Validator : AbstractValidator<Command>
        {
            public Validator()
            {
                RuleFor(v => v.Name)
                    .NotEmpty()
                    .WithMessage("{PropertyName} نباید خالی باشد")
                    .MaximumLength(50)
                    .WithMessage("{PropertyName} نباید بیشتر از 50 کاراکتر باشد");
            }
        }

        public class Handler : IRequestHandler<Command>
        {
            private readonly AppDbContext _dbContext;
            private readonly IMapper _mapper;

            public Handler(AppDbContext dbContext, IMapper mapper)
            {
                _dbContext = dbContext;
                _mapper = mapper;
            }

            public async Task<Unit> Handle(Command request, CancellationToken cancellationToken)
            {
                var role = _mapper.Map<Role>(request);
                _dbContext.Roles.Add(role);
                await _dbContext.SaveChangesAsync();
                return Unit.Value;
            }
        }
    }
}
