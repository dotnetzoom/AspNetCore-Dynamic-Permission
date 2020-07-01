using AutoMapper;
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
    public class UpdateUser
    {
        [HandlerInfo("ویرایش", "کاربر ها", "مدیریت کاربران")]
        public class Command : CreateUser.Command, IRequest
        {
            public int Id { get; set; }
        }

        public class Mapping : Profile
        {
            public Mapping()
            {
                CreateMap<Command, User>();
            }
        }

        public class Validator : AbstractValidator<Command>
        {
            public Validator()
            {
                RuleFor(v => v.FullName)
                    .NotEmpty()
                    .WithMessage("{PropertyName} نباید خالی باشد")
                    .MaximumLength(50)
                    .WithMessage("{PropertyName} نباید بیشتر از 50 کاراکتر باشد");

                RuleFor(v => v.UserName)
                    .NotEmpty()
                    .WithMessage("{PropertyName} نباید خالی باشد")
                    .MaximumLength(50)
                    .WithMessage("{PropertyName} نباید بیشتر از 50 کاراکتر باشد");

                RuleFor(v => v.Password)
                    .NotEmpty()
                    .WithMessage("{PropertyName} نباید خالی باشد");
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
                var userRoles = await _dbContext.UserRoles.Where(p => p.UserId == request.Id).ToListAsync();
                _dbContext.UserRoles.RemoveRange(userRoles);

                var user = await _dbContext.Users.Include(p => p.UserRoles).FirstOrDefaultAsync(p => p.Id == request.Id);
                request.Password = user.Password;
                _mapper.Map(request, user);

                user.UserRoles = new List<UserRole>();
                foreach (var roleId in request.SelectedRoles)
                    user.UserRoles.Add(new UserRole { UserId = user.Id, RoleId = roleId });

                await _dbContext.SaveChangesAsync();
                return Unit.Value;
            }
        }
    }
}
