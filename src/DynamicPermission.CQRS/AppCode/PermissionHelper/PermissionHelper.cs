using DynamicPermission.CQRS.AppCode;
using DynamicPermission.CQRS.Models;
using DynamicPermission.CQRS.ViewModels;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace DynamicPermission.CQRS.App_Code
{
    public static class PermissionHelper
    {
        public static List<PermissionTab> List { get; set; } = new List<PermissionTab>();

        public static IServiceCollection PopulatePermissions(this IServiceCollection services, params Assembly[] assemblies)
        {
            var permissionTabViewModels = new List<PermissionTab>();

            var handlers = assemblies.SelectMany(p => p.GetExportedTypes())
                .Where(type => type.IsPublic && type.IsClass && !type.IsAbstract && type.IsSubclassOf(typeof(IRequestHandler<,>)) && type.GetCustomAttribute<AllowAnonymousAttribute>() == null)
                .Select(handler =>
                {
                    var attribute = handler.GetCustomAttribute<HandlerInfoAttribute>();
                    var namespaceLastSegment = handler.Namespace.Remove(handler.Namespace.IndexOf('.');
                    return new
                    {
                        handler.FullName,
                        TabName = attribute?.TabName ?? namespaceLastSegment,
                        GroupName = attribute.GroupName ?? namespaceLastSegment,
                        Name = attribute.Name ?? handler.Name
                    };
                }).ToList();

            foreach (var tab in handlers.GroupBy(p => p.TabName))
            {
                var tabViewModel = new PermissionTab { Name = tab.Key };
                foreach (var group in tab.GroupBy(p => p.GroupName))
                {
                    var controllerViewModel = new PermissionRequestGroup { Name = group.ControllerName, FullName = group.ControllerFullName };
                    foreach (var action in group.GroupBy(p => p.Name))
                    {
                        var actionViewModel = new PermissionRequest
                        {
                            Name = action.Key,
                            FullNames = action.Select(p => p.ActionFullName).Distinct().ToList()
                        };
                        controllerViewModel.Actions.Add(actionViewModel);
                    }
                    tabViewModel.Controllers.Add(controllerViewModel);
                }
                List.Add(tabViewModel);
            }
        }

        public static List<PermissionTab> GetPermissionTabViewModels(IEnumerable<Permission> permissions)
        {
            var list = new List<PermissionTab>(List);
            foreach (var tab in list)
            {
                foreach (var controller in tab.Controllers)
                {
                    foreach (var action in controller.Actions)
                        action.Selected = permissions.Any(p => action.FullNames.Contains(p.ActionFullName));
                }
            }
            return list;
        }

        private static string GetControllerName(Type type)
        {
            var attribute = type.GetCustomAttribute<HandlerInfoAttribute>();
            return attribute?.Name ?? type.Name.TrimEnd("Controller");
        }

        private static string GetControllerGroupName(Type type)
        {
            var attribute = type.GetCustomAttribute<HandlerInfoAttribute>();
            return attribute?.GroupName ?? GetControllerName(type);
        }

        private static string GetActionName(MethodInfo methodInfo)
        {
            var actionInfo = methodInfo.GetCustomAttribute<HandlerInfoAttribute>();
            if (actionInfo?.Name != null)
                return actionInfo.Name;

            switch (methodInfo.Name)
            {
                case "Index":
                case "Detail":
                case "Details":
                case "Search":
                case "Read":
                    return "مشاهده";
                case "Add":
                case "Insert":
                case "Create":
                    return "افزودن";
                case "Edit":
                case "Put":
                case "Update":
                    return "ویرایش";
                case "Delete":
                case "Remove":
                    return "حذف";
                default:
                    return methodInfo.Name;
            }
        }

        private static string TrimEnd(this string source, string value)
        {
            while (source.EndsWith(value, StringComparison.OrdinalIgnoreCase))
                source = source.Substring(0, source.Length - value.Length);
            return source;
        }
    }
}