using System.Collections.Generic;

namespace DynamicPermission.Mvc5.ViewModels
{
    public class PermissionController
    {
        public string Name { get; set; }
        public string FullName { get; set; }
        public List<PermissionAction> Actions { get; set; } = new List<PermissionAction>();
    }
}
