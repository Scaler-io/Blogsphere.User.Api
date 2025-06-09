using Blogsphere.User.Application.Contracts.ActivityTracker;
using Blogsphere.User.Application.Contracts.Cache;
using Blogsphere.User.Domain.Configurations;
using Blogsphere.User.Domain.Models.Constants;
using Blogsphere.User.Domain.Models.Enums;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;

namespace Blogsphere.User.Infrastructure.Cache;

public class InMemoryCacheService(IMemoryCache memoryCache, IOptions<AppConfigOption> appConfigOption, IActivityTracker activityTracker) : ICacheService
{
    private readonly IMemoryCache _memoryCache = memoryCache;
    private readonly AppConfigOption _appConfigOption = appConfigOption.Value;
    private readonly IActivityTracker _activityTracker = activityTracker;

    public CacheServiceTypes Type { get; } = CacheServiceTypes.InMemory;

    public async Task<bool> ContainsAsync(string key, CancellationToken cancellationToken = default)
    {
        await Task.CompletedTask;
        return _memoryCache.TryGetValue(key, out _);
    }

    public async Task<T?> GetAsync<T>(string cacheKey, CancellationToken cancellationToken = default)
    {
        await Task.CompletedTask;
        using var activity = _activityTracker.TrackInMemoryActivity("GET", cacheKey);
        var data = _memoryCache.Get<T>(cacheKey);

        if (data == null)
        {
            activity?.SetTag(TrackerConstants.CacheHit, false);    
            return default;
        }

        activity?.SetTag(TrackerConstants.CacheHit, true);
        return data;
    }

    public async Task RemoveAsync(string key, CancellationToken cancellationToken = default)
    {
        await Task.CompletedTask;
        _memoryCache.Remove(key);
    }

    public async Task SetAsync<T>(string key, T value, int? expirationTime = null, CancellationToken cancellation = default)
    {
        await Task.CompletedTask;
        using var activity = _activityTracker.TrackInMemoryActivity("SET", key);
        _memoryCache.Set(key, value, new MemoryCacheEntryOptions
        {
            SlidingExpiration = TimeSpan.FromMinutes(10),
            AbsoluteExpirationRelativeToNow = expirationTime.HasValue
            ? TimeSpan.FromMinutes(expirationTime.Value)
            : TimeSpan.FromMinutes(_appConfigOption.CacheExpiration),
        });
        
    }

    public async Task<T?> UpdateAsync<T>(string key, T data)
    {
        await Task.CompletedTask;
        _memoryCache.Set(key, data);
        return _memoryCache.Get<T>(key);
    }
}
