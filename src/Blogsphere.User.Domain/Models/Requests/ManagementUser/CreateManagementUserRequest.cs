namespace Blogsphere.User.Domain.Models.Requests.ManagementUser;

public class CreateManagementUserRequest
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Email { get; set; }
    public string PhoneNumber { get; set; }
    public string Department { get; set; }
    public string JobTitle { get; set; }
    public List<string> Roles { get; set; }
}
