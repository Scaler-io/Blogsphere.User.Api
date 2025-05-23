using Blogsphere.User.Domain.Models.Enums;

namespace Blogsphere.User.Application.Contracts.Security;
public interface IPermissionMapper
{
    List<string> GetPermissionsForRole(ApiAccess role);
}
