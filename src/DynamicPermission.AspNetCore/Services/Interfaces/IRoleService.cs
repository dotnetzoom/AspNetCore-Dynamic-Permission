using DynamicPermission.AspNetCore.Models;
using DynamicPermission.AspNetCore.ViewModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DynamicPermission.AspNetCore.Services
{
    public interface IRoleService
    {
        Task AddAsync(RoleViewModel role);
        Task DeleteAsync(int id);
        Task<List<Role>> GetAllAsync();
        ValueTask<Role> GetByIdAsync(int id);
        Task<Role> GetByIdIncludePermissionsAsync(int id);
        Task UpdateAsync(RoleViewModel role);
    }
}