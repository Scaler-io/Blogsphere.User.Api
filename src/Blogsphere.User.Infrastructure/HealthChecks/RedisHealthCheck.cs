using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace Blogsphere.User.Infrastructure.HealthChecks;

public class RedisHealthCheck(IDistributedCache cache) : IHealthCheck
{
    private readonly IDistributedCache _cache = cache;

    public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
    {
        try
        {
            await _cache.SetStringAsync("redis_health", "OK", new()
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(1),
            }, cancellationToken);
            return HealthCheckResult.Healthy("Redis health check is a success");
        }
        catch (Exception ex)
        {
            return HealthCheckResult.Unhealthy("Redis health check failed", ex);
        }
    }
}
