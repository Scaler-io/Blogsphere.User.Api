using Blogsphere.User.Application.Contracts.Security;
using Blogsphere.User.Domain.Models.Enums;

namespace Blogsphere.User.Application.Security;
public class PermissionMapper : IPermissionMapper
{
    private readonly Dictionary<ApiAccess, List<string>> _map = new()
    {
        {ApiAccess.UserCreate, ["user:create"]},
        {ApiAccess.UserUpdate, ["user:update"]},
        {ApiAccess.UserManageRoles, ["user:manage-roles"]},
        {ApiAccess.UserView, ["user:view"]},
        {ApiAccess.RoleView, ["role:view"]},
        {ApiAccess.RoleCreate, ["role:create"]},
        {ApiAccess.RoleUpdate, ["role:update"]},
        {ApiAccess.RoleAssignPermissions, ["role:assign-permissions"]},
    };

    public List<string> GetPermissionsForRole(ApiAccess role) => _map.TryGetValue(role, out var permissions) ? permissions : [];

}
