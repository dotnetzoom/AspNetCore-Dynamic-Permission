using EFSecondLevelCache;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.Entity;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace DynamicPermission.Mvc5.Models
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(string nameOrConnectionString)
            : base(nameOrConnectionString)
        {
        }

        public AppDbContext(DbConnection existingConnection)
            : base(existingConnection, true)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<UserRole> UserRoles { get; set; }
        public DbSet<Permission> Permissions { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            //modelBuilder.Configurations.AddFromAssembly(typeof(UserConfiguration).Assembly);
        }

        public override int SaveChanges()
        {
            var changedEntityNames = this.GetChangedEntityNames();
            var result = base.SaveChanges();
            new EFCacheServiceProvider().InvalidateCacheDependencies(changedEntityNames);
            return result;
        }

        public override async Task<int> SaveChangesAsync()
        {
            var changedEntityNames = this.GetChangedEntityNames();
            var result = await base.SaveChangesAsync();
            new EFCacheServiceProvider().InvalidateCacheDependencies(changedEntityNames);
            return result;
        }

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken)
        {
            var changedEntityNames = this.GetChangedEntityNames();
            var result = await base.SaveChangesAsync(cancellationToken);
            new EFCacheServiceProvider().InvalidateCacheDependencies(changedEntityNames);
            return result;
        }
    }

    public static class ChangeTrackerExtenstions
    {
        public static string[] GetChangedEntityNames(this DbContext dbContext)
        {
            if (!dbContext.Configuration.AutoDetectChangesEnabled)
            {
                dbContext.ChangeTracker.DetectChanges();
            }

            var typesList = new List<Type>();
            foreach (var type in dbContext.getChangedEntityTypes())
            {
                typesList.Add(type);
                typesList.AddRange(type.getBaseTypes().ToList());
            }

            var changedEntityNames = typesList
                .Select(type => System.Data.Entity.Core.Objects.ObjectContext.GetObjectType(type).FullName)
                .Distinct()
                .ToArray();

            return changedEntityNames;
        }

        private static IEnumerable<Type> getBaseTypes(this Type type)
        {
            if (type.BaseType == null) return type.GetInterfaces();

            return Enumerable.Repeat(type.BaseType, 1)
                             .Concat(type.GetInterfaces())
                             .Concat(type.GetInterfaces().SelectMany(getBaseTypes))
                             .Concat(type.BaseType.getBaseTypes());
        }

        private static IEnumerable<Type> getChangedEntityTypes(this DbContext dbContext)
        {
            return dbContext.ChangeTracker.Entries().Where(
                            dbEntityEntry => dbEntityEntry.State == EntityState.Added ||
                            dbEntityEntry.State == EntityState.Modified ||
                            dbEntityEntry.State == EntityState.Deleted)
                .Select(dbEntityEntry => System.Data.Entity.Core.Objects.ObjectContext.GetObjectType(dbEntityEntry.Entity.GetType()));
        }
    }
}