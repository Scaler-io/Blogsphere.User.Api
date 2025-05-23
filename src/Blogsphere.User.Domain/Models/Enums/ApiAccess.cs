using System.Runtime.Serialization;

namespace Blogsphere.User.Domain.Models.Enums;

public enum ApiAccess
{
    [EnumMember(Value = "user:create")]
    UserCreate,
    [EnumMember(Value = "user:update")]
    UserUpdate,
    [EnumMember(Value = "user:delete")]
    UserDelete,
    [EnumMember(Value = "user:read")]
    UserRead,
}
