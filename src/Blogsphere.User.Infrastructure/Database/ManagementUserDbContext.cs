using Blogsphere.User.Domain.Entities.Management;
using Blogsphere.User.Infrastructure.Database.Configurations.Management;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Blogsphere.User.Infrastructure.Database;

public class ManagementUserDbContext(DbContextOptions<ManagementUserDbContext> options) : IdentityDbContext<ManagementUser, ManagementRole, string, IdentityUserClaim<string>,
ManagementUserRole, IdentityUserLogin<string>, IdentityRoleClaim<string>, IdentityUserToken<string>>(options)
{
    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

                // Set default schema for all management tables
        builder.HasDefaultSchema("Management");
        
        // Apply configurations
        builder.ApplyConfiguration(new ManagementUserEntityConfiguration());
        builder.ApplyConfiguration(new ManagementRoleEntityConfiguration());
        builder.ApplyConfiguration(new ManagementPermissionEntityConfiguration());
        
        // Configure many-to-many relationships
        builder.Entity<ManagementRolePermission>()
            .HasKey(rp => new { rp.RoleId, rp.PermissionId });

        builder.Entity<ManagementUserRole>()
            .HasOne(ur => ur.User)
            .WithMany(u => u.UserRoles)
            .HasForeignKey(ur => ur.UserId);

        builder.Entity<ManagementUserRole>()
            .HasOne(ur => ur.Role)
            .WithMany(r => r.UserRoles)
            .HasForeignKey(ur => ur.RoleId);

        builder.Entity<ManagementRolePermission>()
            .HasOne(rp => rp.Role)
            .WithMany(r => r.RolePermissions)
            .HasForeignKey(rp => rp.RoleId);

        builder.Entity<ManagementRolePermission>() 
            .HasOne(rp => rp.Permission)
            .WithMany(p => p.RolePermissions)
            .HasForeignKey(rp => rp.PermissionId);

        // Set table names (will be in Management schema)
        builder.Entity<ManagementUser>().ToTable("Users");
        builder.Entity<ManagementRole>().ToTable("Roles");
        builder.Entity<ManagementUserRole>().ToTable("UserRoles");
        builder.Entity<ManagementPermission>().ToTable("Permissions");
        builder.Entity<ManagementRolePermission>().ToTable("RolePermissions");
        
        // Override default Identity tables (will be in Management schema)
        builder.Entity<IdentityUserClaim<string>>().ToTable("UserClaims");
        builder.Entity<IdentityUserLogin<string>>().ToTable("UserLogins");
        builder.Entity<IdentityUserToken<string>>().ToTable("UserTokens");
        builder.Entity<IdentityRoleClaim<string>>().ToTable("RoleClaims");
    }

    public DbSet<ManagementPermission> ManagementPermissions { get; set; }
    public DbSet<ManagementRolePermission> ManagementRolePermissions { get; set; }
}
