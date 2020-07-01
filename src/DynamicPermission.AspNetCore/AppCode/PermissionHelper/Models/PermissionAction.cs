using System.Collections.Generic;

namespace DynamicPermission.AspNetCore.ViewModels
{
    public class PermissionAction
    {
        public string Name { get; set; }
        public bool Selected { get; set; }
        public List<string> FullNames { get; set; }
    }
}
