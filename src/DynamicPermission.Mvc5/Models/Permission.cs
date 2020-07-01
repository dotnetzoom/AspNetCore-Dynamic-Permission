using System.Data.Entity.ModelConfiguration;

namespace DynamicPermission.Mvc5.Models
{
    public class Permission
    {
        public int Id { get; set; }
        public string ActionFullName { get; set; }
        public int RoleId { get; set; }
        public Role Role { get; set; }
    }

    public class PermissionConfiguration : EntityTypeConfiguration<Permission>
    {
        public PermissionConfiguration()
        {
            Property(p => p.ActionFullName).HasMaxLength(200);

            HasRequired(p => p.Role)
                .WithMany(p => p.Permissions)
                .HasForeignKey(p => p.RoleId);
        }
    }
}