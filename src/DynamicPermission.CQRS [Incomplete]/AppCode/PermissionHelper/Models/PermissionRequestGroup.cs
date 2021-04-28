using System.Collections.Generic;

namespace DynamicPermission.CQRS.ViewModels
{
    public class PermissionRequestGroup
    {
        public string Name { get; set; }
        public string FullName { get; set; }
        public List<PermissionRequest> PermissionRequests { get; set; } = new List<PermissionRequest>();
    }
}
