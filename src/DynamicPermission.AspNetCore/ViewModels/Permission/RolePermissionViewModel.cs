using System.Collections.Generic;

namespace DynamicPermission.AspNetCore.ViewModels
{
    public class RolePermissionViewModel
    {
        public int RoleId { get; set; }
        public List<string> ActionFullNames { get; set; }
    }
}
