using AutoMapper;
using DynamicPermission.Mvc5.Models;
using DynamicPermission.Mvc5.ViewModels;
using System.Collections.Generic;
using System.Data.Entity;
using System.Threading.Tasks;

namespace DynamicPermission.Mvc5.Services
{
    public class RoleService : IRoleService
    {
        private readonly AppDbContext _dbContext;
        private readonly IMapper _mapper;

        public RoleService(AppDbContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        public Task<Role> GetByIdAsync(int id)
        {
            return _dbContext.Roles.FindAsync(id);
        }

        public Task<List<Role>> GetAllAsync()
        {
            return _dbContext.Roles.ToListAsync();
        }

        public Task<Role> GetByIdIncludePermissionsAsync(int id)
        {
            return _dbContext.Roles.Include(p => p.Permissions).FirstOrDefaultAsync(p => p.Id == id);
        }

        public Task AddAsync(RoleViewModel roleViewModel)
        {
            var role = _mapper.Map<Role>(roleViewModel);
            _dbContext.Roles.Add(role);
            return _dbContext.SaveChangesAsync();
        }

        public async Task UpdateAsync(RoleViewModel roleViewModel)
        {
            var role = await _dbContext.Roles.FindAsync(roleViewModel.Id);
            _mapper.Map(roleViewModel, role);
            await _dbContext.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var role = await _dbContext.Roles.FindAsync(id);
            _dbContext.Roles.Remove(role);
            await _dbContext.SaveChangesAsync();
        }
    }
}