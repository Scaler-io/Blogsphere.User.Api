using Blogsphere.User.Domain.Models.Enums;

namespace Blogsphere.User.Domain.Events;

public abstract class NotificationEventBase : IPublishable
{
    public DateTime CreatedOn { get; set; }
    public string CorrelationId { get; set; }
    public object AdditionalProperties { get; set; }
    protected abstract NotificationType NotificationType { get; set;}
}
