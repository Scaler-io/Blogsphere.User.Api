using Blogsphere.User.Domain.Entities;
using Blogsphere.User.Domain.Models.Responses;

namespace Blogsphere.User.Application.Extensions;

public static class ApplicationUserExtensions
{
    public static List<string> GetUserRoleMappings(this ApplicationUser user)
    {
        ArgumentNullException.ThrowIfNull(user);
        return [.. user.UserRoles.Select(r => r.Role.Name ?? string.Empty)];
    }

    public static List<RoleResponse> GetDetailedUserRoleMappings(this ApplicationUser user)
    {
        return [.. user.UserRoles.Select(r => new RoleResponse { 
            Name = r.Role.Name,
            Description = r.Role.NormalizedName,
            IsSystemRole = false
        })];
    }
    
    public static IEnumerable<string> GetUserPermissionMappings(this ApplicationUser user)
    {
        ArgumentNullException.ThrowIfNull(user);
        return user.UserRoles.SelectMany(r => r.Role.RolePermissions.Select(rp => rp.Permission.Name).ToList()).Distinct();
    }
}
