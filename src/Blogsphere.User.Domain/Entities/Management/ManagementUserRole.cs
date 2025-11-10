using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Blogsphere.User.Domain.Entities.Management;

[PrimaryKey(nameof(UserId), nameof(RoleId))]
public class ManagementUserRole : IdentityUserRole<string>
{
    public ManagementUser User { get; set; } = null!;
    public ManagementRole Role { get; set; } = null!;
    public DateTime AssignedAt { get; set; } = DateTime.UtcNow;
    public string AssignedBy { get; set; } = "System";

    public void SetAssignedBy(string username) => AssignedBy = username;
}
