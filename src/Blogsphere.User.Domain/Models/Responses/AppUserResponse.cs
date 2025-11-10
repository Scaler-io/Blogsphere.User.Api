namespace Blogsphere.User.Domain.Models.Responses;

public class AppUserResponse
{
    public string Id { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string UserName { get; set; }
    public string Email { get; set; }
    public string PhoneNumber { get; set; }
    public bool IsActive { get; set; }
    public bool IsEmailConfirmed { get; set; }
    public bool IsPhoneNumberConfirmed { get; set; }
    public bool IsTwoFactorEnabled { get; set; }
    public string ImageUrl { get; set; } 
    public List<RoleResponse> Roles { get; set; }
    public List<string> Permissions { get; set; }
    public ProfileResponse Profile { get; set; }
    public Metadata Metadata { get; set; }
}
