namespace Blogsphere.User.Domain.Models.Responses;

public class UserRegistrationResponse
{
    public string Id { get; set; }
    public string Email { get; set; }
    public List<string> Role { get; set; }
}
