using Blogsphere.User.Application.Contracts.EventBus;
using Blogsphere.User.Application.Contracts.Factory;
using Blogsphere.User.Domain.Events;
using Microsoft.Extensions.DependencyInjection;

namespace Blogsphere.User.Infrastructure.Factory;

public class PublishServiceFactory(IServiceProvider serviceProvider) : IPublishServiceFactory
{
    private readonly IServiceProvider _serviceProvider = serviceProvider;

    public IPublishService<T, TEvent> CreatePublishService<T, TEvent>()
        where T : class
        where TEvent : IPublishable
    {
        return _serviceProvider.GetRequiredService<IPublishService<T, TEvent>>();
    }
}
