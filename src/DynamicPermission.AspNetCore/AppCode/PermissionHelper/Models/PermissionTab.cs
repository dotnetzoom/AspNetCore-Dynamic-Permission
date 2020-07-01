using System.Collections.Generic;

namespace DynamicPermission.AspNetCore.ViewModels
{
    public class PermissionTab
    {
        public string Name { get; set; }
        public List<PermissionController> Controllers { get; set; } = new List<PermissionController>();
    }
}
