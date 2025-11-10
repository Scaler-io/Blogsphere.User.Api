using System.Runtime.Serialization;

namespace Blogsphere.User.Domain.Models.Enums;

public enum ApiAccess
{
    [EnumMember(Value = "user:create")]
    UserCreate,
    [EnumMember(Value = "user:update")]
    UserUpdate,
    [EnumMember(Value = "user:manage-roles")]
    UserManageRoles,
    [EnumMember(Value = "user:view")]
    UserView,
    [EnumMember(Value = "role:view")]
    RoleView,
    [EnumMember(Value = "role:create")]
    RoleCreate,
    [EnumMember(Value = "role:update")]
    RoleUpdate,
    [EnumMember(Value = "role:assign-permissions")]
    RoleAssignPermissions,
}
