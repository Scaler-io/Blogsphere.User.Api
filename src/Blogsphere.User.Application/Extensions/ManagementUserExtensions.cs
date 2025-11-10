using Blogsphere.User.Domain.Entities.Management;
using Blogsphere.User.Domain.Models.Responses;

namespace Blogsphere.User.Application.Extensions;

public static class ManagementUserExtensions
{
    public static List<string> GetUserRoleMappings(this ManagementUser user)
    {
        ArgumentNullException.ThrowIfNull(user);
        return [.. user.UserRoles.Select(r => r.Role.Name ?? string.Empty)];
    }

    public static List<RoleResponse> GetDetailedUserRoleMappings(this ManagementUser user)
    {
        return [.. user.UserRoles.Select(static r => new RoleResponse 
        { 
            Name = r.Role.Name, 
            Description = r.Role.Description, 
            IsSystemRole = r.Role.IsSystemRole
        })];
    }

    public static IEnumerable<string> GetUserPermissionMappings(this ManagementUser user)
    {
        ArgumentNullException.ThrowIfNull(user);
        return user.UserRoles.SelectMany(r => r.Role.RolePermissions.Select(rp => rp.Permission.Name).ToList()).Distinct();
    }
}
