using Blogsphere.User.Domain.Entities;
using Blogsphere.User.Infrastructure.Database.Configurations;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Blogsphere.User.Infrastructure.Database;

public class UserDbContext(DbContextOptions<UserDbContext> options) : IdentityDbContext<ApplicationUser, ApplicationRole, string, 
    IdentityUserClaim<string>, ApplicationUserRole, 
    IdentityUserLogin<string>, IdentityRoleClaim<string>, 
    IdentityUserToken<string>>(options)
{
    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        builder.ApplyConfigurationsFromAssembly(typeof(ApplicationUserEntityConfiguration).Assembly);

        builder.Entity<ApplicationRolePermission>()
        .HasKey(ck => new { ck.RoleId, ck.PermissionId });
    }

    public DbSet<ApplicationPermission> Permissions { get; set; }
    public DbSet<ApplicationRolePermission> RolePermissions { get; set; }
}
