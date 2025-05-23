using Microsoft.AspNetCore.Identity;

namespace Blogsphere.User.Domain.Entities;

public class ApplicationUserRole : IdentityUserRole<string>
{
    public ApplicationUser User { get; set; }
    public ApplicationRole Role { get; set; }
}
