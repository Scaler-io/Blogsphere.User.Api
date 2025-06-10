using Blogsphere.User.Application.Contracts.Cache;
using Blogsphere.User.Domain.Models.Enums;

namespace Blogsphere.User.Application.Contracts.Factory;

public interface ICacheServiceFactory
{
    ICacheService Create(CacheServiceTypes type);
}
