using DynamicPermission.Mvc5.Models;
using DynamicPermission.Mvc5.ViewModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DynamicPermission.Mvc5.Services
{
    public interface IRoleService
    {
        Task AddAsync(RoleViewModel role);
        Task DeleteAsync(int id);
        Task<List<Role>> GetAllAsync();
        Task<Role> GetByIdAsync(int id);
        Task<Role> GetByIdIncludePermissionsAsync(int id);
        Task UpdateAsync(RoleViewModel role);
    }
}