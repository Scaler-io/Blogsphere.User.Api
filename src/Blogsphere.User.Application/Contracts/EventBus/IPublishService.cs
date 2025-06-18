using Blogsphere.User.Domain.Events;

namespace Blogsphere.User.Application.Contracts.EventBus;

public interface IPublishService<T, TEvent>
    where T : class
    where TEvent : IPublishable
{
    Task PublishAsync(T message, string correlationId, object additionalProperties = null);
}
