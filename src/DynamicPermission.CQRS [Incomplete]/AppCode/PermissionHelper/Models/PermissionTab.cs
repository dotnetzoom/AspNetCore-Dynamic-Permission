using System.Collections.Generic;

namespace DynamicPermission.CQRS.ViewModels
{
    public class PermissionTab
    {
        public string Name { get; set; }
        public List<PermissionRequestGroup> PermissionRequestGroups { get; set; } = new List<PermissionRequestGroup>();
    }
}
