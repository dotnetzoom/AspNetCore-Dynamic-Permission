using AutoMapper;
using AutoMapper.QueryableExtensions;
using DynamicPermission.AspNetCore.AppCode;
using DynamicPermission.AspNetCore.Models;
using DynamicPermission.AspNetCore.ViewModels;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DynamicPermission.AspNetCore.Services
{
    public class UserService : IUserService
    {
        private readonly AppDbContext _dbContext;
        private readonly IMapper _mapper;

        public UserService(AppDbContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        public Task<UserViewModel> GetByIdWithRolesAsync(int id)
        {
            return _dbContext.Users
                .ProjectTo<UserViewModel>(_mapper.ConfigurationProvider)
                .FirstOrDefaultAsync(p => p.Id == id);
        }

        public Task<List<UserViewModel>> GetAllWithRolesAsync()
        {
            return _dbContext.Users
                .ProjectTo<UserViewModel>(_mapper.ConfigurationProvider)
                .ToListAsync();
        }

        public Task AddAsync(UserViewModel userViewModel)
        {
            var user = _mapper.Map<User>(userViewModel);
            user.Password = user.Password.GetMd5Hash();

            user.UserRoles = new List<UserRole>();
            foreach (var roleId in userViewModel.SelectedRoles)
                user.UserRoles.Add(new UserRole { RoleId = roleId });

            _dbContext.Users.Add(user);
            return _dbContext.SaveChangesAsync();
        }

        public async Task UpdateAsync(UserViewModel userViewModel)
        {
            var userRoles = await _dbContext.UserRoles.Where(p => p.UserId == userViewModel.Id).ToListAsync();
            _dbContext.UserRoles.RemoveRange(userRoles);

            var user = await _dbContext.Users.Include(p => p.UserRoles).FirstOrDefaultAsync(p => p.Id == userViewModel.Id);
            userViewModel.Password = user.Password;
            _mapper.Map(userViewModel, user);

            user.UserRoles = new List<UserRole>();
            foreach (var roleId in userViewModel.SelectedRoles)
                user.UserRoles.Add(new UserRole { UserId = user.Id, RoleId = roleId });

            await _dbContext.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var user = await _dbContext.Users.FindAsync(id);
            _dbContext.Users.Remove(user);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<bool> ValidateLoginAsync(UserLoginViewModel userLoginViewModel)
        {
            var password = userLoginViewModel.Password.GetMd5Hash();
            var exists = await _dbContext.Users.AnyAsync(p => p.UserName == userLoginViewModel.UserName && p.Password == password);
            return exists;
        }
    }
}