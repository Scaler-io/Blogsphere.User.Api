using Blogsphere.User.Application.Contracts.Cache;
using Blogsphere.User.Application.Contracts.Factory;
using Blogsphere.User.Domain.Models.Enums;
using Blogsphere.User.Infrastructure.Cache;
using Microsoft.Extensions.DependencyInjection;

namespace Blogsphere.User.Infrastructure.Factory;

public class CacheServiceFactory(IServiceProvider serviceProvider) : ICacheServiceFactory
{
    private readonly IServiceProvider _serviceProvider = serviceProvider;

    public ICacheService Create(CacheServiceTypes type)
    {
        return type switch
        {
            CacheServiceTypes.InMemory => _serviceProvider.GetRequiredService<InMemoryCacheService>(),
            CacheServiceTypes.Distributed => _serviceProvider.GetRequiredService<DistributedCacheService>(),
            _ => throw new ArgumentException("No such cache service is defined")
        };
    }
}
