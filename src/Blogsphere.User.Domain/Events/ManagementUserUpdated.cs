using Blogsphere.User.Domain.Events;
using Blogsphere.User.Domain.Models.Enums;

namespace Contracts.Events;

public class ManagementUserUpdated : GenericEvent
{
    public string Id { get; set; }
    public List<string> Roles { get; set; }
    public string Status { get; set; }
    protected override GenericEventType Type { get; set; } = GenericEventType.ManagementUserUpdated;
}
