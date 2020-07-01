using System.Data.Entity.ModelConfiguration;

namespace DynamicPermission.Mvc5.Models
{
    public class UserRole
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int RoleId { get; set; }

        public User User { get; set; }
        public Role Role { get; set; }
    }

    public class UserRoleConfiguration : EntityTypeConfiguration<UserRole>
    {
        public UserRoleConfiguration()
        {
            HasRequired(p => p.User)
                .WithMany(p => p.UserRoles)
                .HasForeignKey(p => p.UserId);

            HasRequired(p => p.Role)
                .WithMany(p => p.UserRoles)
                .HasForeignKey(p => p.RoleId);
        }
    }
}