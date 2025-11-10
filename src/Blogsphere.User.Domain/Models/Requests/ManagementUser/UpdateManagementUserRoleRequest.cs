namespace Blogsphere.User.Domain.Models.Requests.ManagementUser;

public class UpdateManagementUserRoleRequest
{
    public string Id { get; set; }
    public string Role { get; set; }
    public bool IsActive { get; set; }
}
