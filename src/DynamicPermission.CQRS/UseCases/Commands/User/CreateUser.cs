using AutoMapper;
using DynamicPermission.CQRS.AppCode;
using DynamicPermission.CQRS.Models;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Threading;
using System.Threading.Tasks;

namespace DynamicPermission.CQRS.UseCases
{
    public class CreateUser
    {
        [HandlerInfo("افزودن", "کاربر ها", "مدیریت کاربران")]
        public class Command : IRequest
        {
            //[Display(Name = "نام و نام خانوادگی")]
            public string FullName { get; set; }

            //[Display(Name = "نام کاربری")]
            public string UserName { get; set; }

            //[Display(Name = "رمز عبور")]
            public string Password { get; set; }

            [Display(Name = "نقش ها")]
            public List<int> SelectedRoles { get; set; }

            public List<GetAllRoles.ViewModel> AllRoles { get; set; }

            public SelectList GetRolesSelectList()
            {
                return new SelectList(AllRoles, nameof(GetAllRoles.ViewModel.Id), nameof(GetAllRoles.ViewModel.Name));
            }
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
                    .WithMessage("{PropertyName} نباید بیشتر از 50 کاراکتر باشد")
                    .WithName("نام و نام خانوادگی");

                RuleFor(v => v.UserName)
                    .NotEmpty()
                    .WithMessage("{PropertyName} نباید خالی باشد")
                    .MaximumLength(50)
                    .WithMessage("{PropertyName} نباید بیشتر از 50 کاراکتر باشد")
                    .WithName("نام کاربری");

                RuleFor(v => v.Password)
                    .NotEmpty()
                    .WithMessage("{PropertyName} نباید خالی باشد")
                    .WithName("رمز عبور");
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
                var user = _mapper.Map<User>(request);
                user.Password = user.Password.GetMd5Hash();

                user.UserRoles = new List<UserRole>();
                foreach (var roleId in request.SelectedRoles)
                    user.UserRoles.Add(new UserRole { RoleId = roleId });

                _dbContext.Users.Add(user);
                await _dbContext.SaveChangesAsync();
                return Unit.Value;
            }
        }
    }
}
