using Blogsphere.User.Domain.Models.Enums;

namespace Blogsphere.User.Domain.Events;

public abstract class GenericEvent : IPublishable
{
    public DateTime CreatedAt { get; set; }   
    public DateTime LastUpdatedAt { get; set; }
    public string CorrelationId { get; set; }
    public object AdditionalProperties { get; set; }
    protected abstract GenericEventType Type { get; set;}
}
