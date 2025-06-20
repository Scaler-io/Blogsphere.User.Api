using AutoMapper;
using Blogsphere.User.Application.Contracts.EventBus;
using Blogsphere.User.Domain.Events;
using MassTransit;
using Blogsphere.User.Application.Extensions;
using Serilog;

namespace Blogsphere.User.Infrastructure.EventBus;

public class PublishService<T, TEvent>(IPublishEndpoint publishEndpoint, IMapper mapper, ILogger logger) : IPublishService<T, TEvent>
    where T : class
    where TEvent : IPublishable
{
    private readonly IPublishEndpoint _publishEndpoint = publishEndpoint;
    private readonly IMapper _mapper = mapper;
    private readonly ILogger _logger = logger;

    public async Task PublishAsync(T message, string correlationId, object additionalProperties = default)
    {
        _logger.Here().MethodEntered();
        var newEvent = _mapper.Map<TEvent>(message);
        newEvent.CorrelationId = correlationId;
        newEvent.AdditionalProperties = additionalProperties;

        await _publishEndpoint.Publish(newEvent);

        _logger.Here()
        .WithCorrelationId(correlationId)
        .Information("Successfully published {messageType} event message", typeof(TEvent).Name);
    }
}
