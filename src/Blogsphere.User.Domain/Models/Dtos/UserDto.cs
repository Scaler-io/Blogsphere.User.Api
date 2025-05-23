namespace Blogsphere.User.Domain.Models.Dtos;

public class UserDto
{
    public string Id { get; set; }
    public string UserName { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Email { get; set; }
    public AuthorizationDto Authorization { get; set; }   

    public bool IsAdmin() => Authorization.Roles.Contains("Admin");
    public bool IsEditor() => Authorization.Roles.Contains("Editor");
    public bool IsAuthor() => Authorization.Roles.Contains("Author");
    public bool IsSubscriber() => Authorization.Roles.Contains("Subscriber");
}
