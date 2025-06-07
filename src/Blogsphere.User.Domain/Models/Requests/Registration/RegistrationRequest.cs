using Blogsphere.User.Domain.Models.Enums;

namespace Blogsphere.User.Domain.Models.Requests.Registration;

public class RegistrationRequest
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }
    public string ConfirmPassword { get; set; }
    public Roles Role { get; set; } = Roles.Subscriber;

    // profile details
    public string Bio { get; set; } = string.Empty;
    public string WebsiteUrl { get; set; } = string.Empty;
    public string LinkedIn { get; set; } = string.Empty;
    public string Twitter { get; set; } = string.Empty;
    public string Instagram { get; set; } = string.Empty;
}

