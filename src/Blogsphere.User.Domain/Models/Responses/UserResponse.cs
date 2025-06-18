namespace Blogsphere.User.Domain.Models.Responses;

public class UserResponse
{
    public string Id { get; set; }
    public string Email { get; set; }
    public List<string> Role { get; set; }
}
