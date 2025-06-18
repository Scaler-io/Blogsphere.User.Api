using Blogsphere.User.Domain.Events;
using Blogsphere.User.Domain.Models.Enums;

namespace Contracts.Events;

public sealed class UserInvitationSent : NotificationEventBase
{
    public string UserId { get; set; }
    public string UserName { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Email { get; set; }
    protected override NotificationType NotificationType {get; set;} = NotificationType.UserInvitationSent;
}
