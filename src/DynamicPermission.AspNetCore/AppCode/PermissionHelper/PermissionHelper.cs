using DynamicPermission.AspNetCore.Models;
using DynamicPermission.AspNetCore.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace DynamicPermission.AspNetCore.App_Code
{
    public static class PermissionHelper
    {
        public static List<PermissionTab> Tabs { get; set; } = new List<PermissionTab>();

        static PermissionHelper()
        {
            var permissionTabViewModels = new List<PermissionTab>();

            var assembly = Assembly.GetExecutingAssembly();
            var controllers = assembly.GetExportedTypes()
                .Where(type => type.IsPublic && type.IsClass && !type.IsAbstract && type.IsSubclassOf(typeof(ControllerBase)) && type.GetCustomAttribute<AllowAnonymousAttribute>() == null)
                .Select(controller => new
                {
                    ControllerFullName = controller.FullName,
                    ControllerName = GetControllerName(controller),
                    ControllerGroupName = GetControllerGroupName(controller),
                    Actions = controller.GetMethods(BindingFlags.Instance | BindingFlags.DeclaredOnly | BindingFlags.Public)
                        .Where(method => method.IsPublic && method.GetCustomAttribute<NonActionAttribute>() == null && method.GetCustomAttribute<AllowAnonymousAttribute>() == null)
                        .Select(method => new
                        {
                            ActionName = GetActionName(method),
                            ActionFullName = controller.FullName + "." + method.Name
                        }).ToList(),
                }).ToList();

            foreach (var tab in controllers.GroupBy(p => p.ControllerGroupName))
            {
                var tabViewModel = new PermissionTab { Name = tab.Key };
                foreach (var controller in tab)
                {
                    var controllerViewModel = new PermissionController { Name = controller.ControllerName, FullName = controller.ControllerFullName };
                    foreach (var action in controller.Actions.GroupBy(p => p.ActionName))
                    {
                        var actionViewModel = new PermissionAction
                        {
                            Name = action.Key,
                            FullNames = action.Select(p => p.ActionFullName).Distinct().ToList()
                        };
                        controllerViewModel.Actions.Add(actionViewModel);
                    }
                    tabViewModel.Controllers.Add(controllerViewModel);
                }
                Tabs.Add(tabViewModel);
            }
        }

        public static List<PermissionTab> GetPermissionTabViewModels(IEnumerable<Permission> permissions)
        {
            var list = new List<PermissionTab>(Tabs);
            foreach (var tab in list)
            {
                foreach (var controller in tab.Controllers)
                {
                    foreach (var action in controller.Actions)
                        action.Selected = permissions.Any(p => action.FullNames.Contains( p.ActionFullName));
                }
            }
            return list;
        }
        private static string GetControllerName(Type type)
        {
            var attribute = type.GetCustomAttribute<ControllerInfoAttribute>();
            return attribute?.Name ?? type.Name.TrimEnd("Controller");
        }

        private static string GetControllerGroupName(Type type)
        {
            var attribute = type.GetCustomAttribute<ControllerInfoAttribute>();
            return attribute?.GroupName ?? GetControllerName(type);
        }

        private static string GetActionName(MethodInfo methodInfo)
        {
            var actionInfo = methodInfo.GetCustomAttribute<ActionInfoAttribute>();
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