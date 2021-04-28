using System.Collections.Generic;

namespace DynamicPermission.CQRS.ViewModels
{
    public class PermissionRequest
    {
        public string Name { get; set; }
        public bool Selected { get; set; }
        public List<string> FullNames { get; set; }
    }
}
