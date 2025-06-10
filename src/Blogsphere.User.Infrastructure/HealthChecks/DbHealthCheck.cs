using Blogsphere.User.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace Blogsphere.User.Infrastructure.HealthChecks;
public sealed class DbHealthCheck(UserDbContext userDbContext) : IHealthCheck
{
    private readonly UserDbContext _userDbContext = userDbContext;

    public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
    {
        try
        {
            var result = await _userDbContext.Users.AnyAsync();
            return HealthCheckResult.Healthy("Db health check is a success");
        }
        catch (Exception ex)
        {
            return HealthCheckResult.Unhealthy("Db health check failed", ex);
        }
    }
}
