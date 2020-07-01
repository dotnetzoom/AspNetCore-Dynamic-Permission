using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;

namespace DynamicPermission.Mvc5.Models
{
    public class Role
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public ICollection<Permission> Permissions { get; set; }
        public ICollection<UserRole> UserRoles { get; set; }
    }

    public class RoleConfiguration : EntityTypeConfiguration<Role>
    {
        public RoleConfiguration()
        {
            Property(p => p.Name).HasMaxLength(50);
        }
    }
}