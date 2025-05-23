using Microsoft.AspNetCore.DataProtection.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Blogsphere.User.Infrastructure.Database;
public sealed class DataProtectionKeyContext(DbContextOptions<DataProtectionKeyContext> options) 
    : DbContext(options), IDataProtectionKeyContext
{
    public DbSet<DataProtectionKey> DataProtectionKeys { get; set; }
}
