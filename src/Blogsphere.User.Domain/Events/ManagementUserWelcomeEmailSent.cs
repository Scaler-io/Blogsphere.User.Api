using Blogsphere.User.Domain.Events;
using Blogsphere.User.Domain.Models.Enums;

namespace Contracts.Events;

public class ManagementUserWelcomeEmailSent : NotificationEventBase
{
    public string FullName { get; set; }
    public string Email { get; set; }
    public string Role { get; set; }
    protected override NotificationType NotificationType { get; set; } = NotificationType.ManagementUserWelcomeEmailSent;
}
