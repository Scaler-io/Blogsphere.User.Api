using Blogsphere.User.Application.Contracts.Security;
using Blogsphere.User.Domain.Entities;
using Blogsphere.User.Domain.Models.Enums;
using Microsoft.AspNetCore.Identity;

namespace Blogsphere.User.Application.Security;
public class PermissionMapper : IPermissionMapper
{
    private readonly Dictionary<ApiAccess, List<string>> _map = [];

    public PermissionMapper(RoleManager<ApplicationRole> roleManager)
    {
        var permissions = roleManager.Roles
            .SelectMany(r => r.RolePermissions.Select(rp => rp.Permission.Name).ToList())
            .ToList()
            .Distinct();

        foreach(var permission in permissions)
        {
            string[] parts = permission.Split(':');

            if(parts.Length == 2 && Enum.TryParse<ApiAccess>(ToPascalCase(parts[0], parts[1]), out var apiAccess))
            {
                if (!_map.TryGetValue(apiAccess, out var value))
                {
                    value = [];
                    _map[apiAccess] = value;
                }

                value.Add(permission);
            }
        }

    }

    private static string ToPascalCase(string first, string second) => $"{char.ToUpper(first[0])}{first[1..]}{char.ToUpper(second[0])}{second[1..]}";

    public List<string> GetPermissionsForRole(ApiAccess role) => _map[role];

}
