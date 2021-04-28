using AutoMapper;
using DynamicPermission.CQRS.AppCode;
using DynamicPermission.CQRS.Models;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.Threading;
using System.Threading.Tasks;

namespace DynamicPermission.CQRS.UseCases
{
    public class ValidateUserLogin
    {
        [AllowAnonymous]
        public class Query : IRequest<bool>
        {
            [Display(Name = "نام کاربری")]
            public string UserName { get; set; }

            [Display(Name = "رمز عبور")]
            public string Password { get; set; }
        }

        public class Validator : AbstractValidator<Query>
        {
            public Validator()
            {
                RuleFor(v => v.UserName)
                    .NotEmpty()
                    .WithMessage("{PropertyName} نباید خالی باشد")
                    .MaximumLength(50)
                    .WithMessage("{PropertyName} نباید بیشتر از 50 کاراکتر باشد")
                    .WithName("نام و نام خانوادگی");

                RuleFor(v => v.Password)
                    .NotEmpty()
                    .WithMessage("{PropertyName} نباید خالی باشد")
                    .WithName("رمز عبور");
            }
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

        public class Handler : IRequestHandler<Query, bool>
        {
            private readonly AppDbContext _dbContext;

            public Handler(AppDbContext dbContext)
            {
                _dbContext = dbContext;
            }

            public async Task<bool> Handle(Query request, CancellationToken cancellationToken)
            {
                var password = request.Password.GetMd5Hash();
                var exists = await _dbContext.Users.AnyAsync(p => p.UserName == request.UserName && p.Password == password);
                return exists;
            }
        }
    }
}
