using AutoMapper;
using DynamicPermission.Mvc5;
using DynamicPermission.Mvc5.Models;
using DynamicPermission.Mvc5.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Owin;
using Microsoft.Owin.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

[assembly: PreApplicationStartMethod(typeof(MvcApplication), "InitModule")]
namespace DynamicPermission.Mvc5
{
    public class MvcApplication : HttpApplication
    {
        public static void InitModule()
        {
            RegisterModule(typeof(ServiceScopeModule));
        }

        protected void Application_Start()
        {
            var services = new ServiceCollection();
            ConfigureServices(services);
            var serviceProvider = services.BuildServiceProvider();
            using (var scope = serviceProvider.CreateScope())
            {
                var seedService = scope.ServiceProvider.GetRequiredService<ISeedService>();
                seedService.SeedAsync().GetAwaiter().GetResult();
            }

            ServiceScopeModule.SetServiceProvider(serviceProvider);
            DependencyResolver.SetResolver(new ServiceScopeDependencyResolver());

            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }

        public void ConfigureServices(IServiceCollection services)
        {
            //====================================================
            // Add DbContext with InMemory (Effort) provider
            //====================================================
            var connection = Effort.DbConnectionFactory.CreateTransient();
            services.AddScoped(_ =>
            {
                return new AppDbContext(connection);
            });

            //====================================================
            // ApplicationSignInManager
            //====================================================
            // instantiation requires two parameters, [ApplicationUserManager] (defined above) and [IAuthenticationManager]
            services.AddTransient(typeof(IAuthenticationManager), p => new OwinContext().Authentication);
            //services.AddTransient(typeof(IAuthenticationManager), p => HttpContext.Current.GetOwinContext().Authentication);

            //====================================================
            // Add all controllers as services
            //====================================================
            var controllerTypes = Assembly.GetExecutingAssembly().GetExportedTypes()
                .Where(p => p.IsClass && !p.IsAbstract && !p.IsGenericTypeDefinition &&
                typeof(IController).IsAssignableFrom(p) || p.Name.EndsWith("Controller", StringComparison.OrdinalIgnoreCase)).ToList();

            foreach (var type in controllerTypes)
            {
                services.AddTransient(type);
            }

            services.AddAutoMapper(Assembly.GetExecutingAssembly());

            services.AddScoped<IRoleService, RoleService>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IPermissionService, PermissionService>(); 
            services.AddScoped<ISeedService, SeedService>(); 
        }
    }

    public class ServiceScopeModule : IHttpModule
    {
        private static IServiceProvider _serviceProvider;

        public static void SetServiceProvider(ServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public void Init(HttpApplication context)
        {
            context.BeginRequest += Context_BeginRequest;
            context.EndRequest += Context_EndRequest;
        }

        private void Context_BeginRequest(object sender, EventArgs e)
        {
            var context = ((HttpApplication)sender).Context;
            context.Items[typeof(IServiceScope)] = _serviceProvider.CreateScope();
        }

        private void Context_EndRequest(object sender, EventArgs e)
        {
            var context = ((HttpApplication)sender).Context;
            if (context.Items[typeof(IServiceScope)] is IServiceScope scope)
            {
                scope.Dispose();
            }
        }

        public void Dispose()
        {
        }
    }

    public class ServiceScopeDependencyResolver : IDependencyResolver
    {
        public object GetService(Type serviceType)
        {
            if (HttpContext.Current?.Items[typeof(IServiceScope)] is IServiceScope scope)
            {
                return scope.ServiceProvider.GetService(serviceType);
            }

            throw new InvalidOperationException("IServiceScope not provided");
        }

        public IEnumerable<object> GetServices(Type serviceType)
        {
            if (HttpContext.Current?.Items[typeof(IServiceScope)] is IServiceScope scope)
            {
                return scope.ServiceProvider.GetServices(serviceType);
            }

            throw new InvalidOperationException("IServiceScope not provided");
        }
    }
}
