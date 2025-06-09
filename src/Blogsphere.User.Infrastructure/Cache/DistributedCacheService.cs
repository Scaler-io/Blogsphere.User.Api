using Blogsphere.User.Application.Contracts.ActivityTracker;
using Blogsphere.User.Application.Contracts.Cache;
using Blogsphere.User.Domain.Configurations;
using Blogsphere.User.Domain.Models.Constants;
using Blogsphere.User.Domain.Models.Enums;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;

namespace Blogsphere.User.Infrastructure.Cache;

public class DistributedCacheService(IDistributedCache distributedCache, IOptions<AppConfigOption> appConfigOption, IActivityTracker activityTracker) : ICacheService
{

    private readonly IDistributedCache _distributedCache = distributedCache;
    private readonly AppConfigOption _appConfigOption = appConfigOption.Value;
    private readonly IActivityTracker _activityTracker = activityTracker;

    public CacheServiceTypes Type { get; } = CacheServiceTypes.Distributed;

    public async Task<T?> GetAsync<T>(string cacheKey, CancellationToken cancellationToken = default)
    {
        using var activity = _activityTracker.TrackRedisActivity("GET", cacheKey);
        var data = await _distributedCache.GetStringAsync(cacheKey, cancellationToken);
        if (data is null)
        {
            activity?.SetTag(TrackerConstants.CacheHit, false);
            return default;
        }

        activity?.SetTag(TrackerConstants.CacheHit, true);
        return JsonConvert.DeserializeObject<T>(data);
    }

    public async Task SetAsync<T>(string key, T value, int? expirationTime = null, CancellationToken cancellation = default)
    {
        using var activity = _activityTracker.TrackRedisActivity("SET", key);

        var serializeData = JsonConvert.SerializeObject(value);
        var cacheOptions = new DistributedCacheEntryOptions();
        cacheOptions.SetAbsoluteExpiration(TimeSpan.FromMinutes(expirationTime ?? _appConfigOption.CacheExpiration));
        await _distributedCache.SetStringAsync(key, serializeData, cacheOptions, cancellation);
    }

    public async Task<bool> ContainsAsync(string key, CancellationToken cancellationToken = default)
    {
        return (await _distributedCache.GetStringAsync(key, cancellationToken)) is not null;
    }

    public async Task<T?> UpdateAsync<T>(string key, T data)
    {
        await _distributedCache.SetStringAsync(key, JsonConvert.SerializeObject(data));
        return await GetAsync<T>(key);
    }

    public async Task RemoveAsync(string key, CancellationToken cancellationToken = default)
    {
        await _distributedCache.RemoveAsync(key, cancellationToken);
    }
}
