using DynamicPermission.CQRS.App_Code;
using DynamicPermission.CQRS.Models;
using DynamicPermission.CQRS.UseCases;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DynamicPermission.CQRS.Services
{
    public class SeedService : ISeedService
    {
        private readonly CreateRole.Handler _createRoleHandler;
        private readonly CreateUser.Handler _createUserHandler;
        private readonly AppDbContext _appDbContext;

        public SeedService(AppDbContext appDbContext, CreateRole.Handler createRoleHandler, CreateUser.Handler createUserHandler)
        {
            _appDbContext = appDbContext;
            _createRoleHandler = createRoleHandler;
            _createUserHandler = createUserHandler;
        }

        public async Task SeedAsync()
        {
            await _createRoleHandler.Handle(new CreateRole.Command 
            {
                Name = "مدیر"
            }, default);
            await _createRoleHandler.Handle(new CreateRole.Command
            {
                Name = "نویسنده"
            }, default);

            await _createUserHandler.Handle(new CreateUser.Command
            {
                FullName = "محمد جواد ابراهیمی",
                Password = "admin",
                UserName = "admin",
                SelectedRoles = new List<int> { 1 }
            }, default);
            await _createUserHandler.Handle(new CreateUser.Command
            {
                FullName = "کاربر تست",
                Password = "writer",
                UserName = "writer",
                SelectedRoles = new List<int> { 2 }
            }, default);

            var allActionFullNames = PermissionHelper.List
                .SelectMany(tab => tab.Controllers.SelectMany(controller => controller.Actions.SelectMany(action => action.FullNames)))
                .Distinct().ToList();

            var permissions = allActionFullNames.Select(p => new Permission { ActionFullName = p, RoleId = 1 }).ToList();
            await _appDbContext.Permissions.AddRangeAsync(permissions);
        }
    }
}