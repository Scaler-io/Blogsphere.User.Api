using Blogsphere.User.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Blogsphere.User.Infrastructure.Database.Configurations;

public class ApplicationRoleEntityConfiguration : IEntityTypeConfiguration<ApplicationRole>
{
    public void Configure(EntityTypeBuilder<ApplicationRole> builder)
    {
        builder.HasMany(r => r.UserRoles)
            .WithOne(ur => ur.Role)
            .HasForeignKey(fk => fk.RoleId)
            .IsRequired();

        builder.HasMany(rp => rp.RolePermissions)
            .WithOne(r => r.Role)
            .HasForeignKey(fk => fk.RoleId)
            .IsRequired();
    }
}
