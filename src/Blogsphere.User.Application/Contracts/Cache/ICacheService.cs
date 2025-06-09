using Blogsphere.User.Domain.Models.Enums;

namespace Blogsphere.User.Application.Contracts.Cache;

public interface ICacheService
{
    CacheServiceTypes Type { get; }
    Task<T?> GetAsync<T>(string cacheKey, CancellationToken cancellationToken = default);
    Task SetAsync<T>(string key, T value, int? expirationTime = null, CancellationToken cancellation = default);
    Task<bool> ContainsAsync(string key, CancellationToken cancellationToken = default);
    Task<T?> UpdateAsync<T>(string key, T data);
    Task RemoveAsync(string key, CancellationToken cancellationToken = default);
}
