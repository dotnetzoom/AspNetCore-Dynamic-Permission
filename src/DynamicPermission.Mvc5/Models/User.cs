using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;

namespace DynamicPermission.Mvc5.Models
{
    public class User
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string FullName { get; set; }
        public ICollection<UserRole> UserRoles { get; set; }
    }

    public class UserConfiguration : EntityTypeConfiguration<User>
    {
        public UserConfiguration()
        {
            Property(p => p.FullName).HasMaxLength(50);
            Property(p => p.UserName).HasMaxLength(50);
        }
    }
}