using Blogsphere.User.Application.Contracts.EventBus;
using Blogsphere.User.Domain.Events;

namespace Blogsphere.User.Application.Contracts.Factory;

public interface IPublishServiceFactory
{
    IPublishService<T, TEvent> CreatePublishService<T, TEvent>()
        where T : class
        where TEvent : IPublishable;
}
