using DynamicPermission.Mvc5.ViewModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DynamicPermission.Mvc5.Services
{
    public interface IUserService
    {
        Task AddAsync(UserViewModel userViewModel);
        Task DeleteAsync(int id);
        Task<List<UserViewModel>> GetAllWithRolesAsync();
        Task<UserViewModel> GetByIdWithRolesAsync(int id);
        Task UpdateAsync(UserViewModel userViewModel);
        Task<bool> ValidateLoginAsync(UserLoginViewModel userLoginViewModel);
    }
}