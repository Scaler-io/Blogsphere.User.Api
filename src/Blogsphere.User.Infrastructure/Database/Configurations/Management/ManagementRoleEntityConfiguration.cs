using Blogsphere.User.Domain.Entities.Management;
using Microsoft.EntityFrameworkCore;

namespace Blogsphere.User.Infrastructure.Database.Configurations.Management;

public class ManagementRoleEntityConfiguration : IEntityTypeConfiguration<ManagementRole>
{
    public void Configure(Microsoft.EntityFrameworkCore.Metadata.Builders.EntityTypeBuilder<ManagementRole> builder)
    {
        builder.Property(r => r.Description)
            .HasMaxLength(500);

        builder.Property(r => r.Name)
            .HasMaxLength(256)
            .IsRequired();

        builder.Property(r => r.NormalizedName)
            .HasMaxLength(256);
    }
}
