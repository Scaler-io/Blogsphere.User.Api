using Blogsphere.User.Application.Contracts.Data.Repositories;
using Blogsphere.User.Domain.Entities.Management;
using Blogsphere.User.Application.Extensions;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using IdentityModel;
using Newtonsoft.Json;
using System.Security.Claims;

namespace Blogsphere.User.Infrastructure.Database.Repositories.Management;

public class ManagementUserRepository(UserManager<ManagementUser> userManager) : IManagementUserRepository
{
    private readonly UserManager<ManagementUser> _userManager = userManager;


    public async Task<bool> AddToClaimsAsync(string userName)
    {
        var user = await _userManager.Users
            .Include("UserRoles.Role.RolePermissions.Permission")
            .FirstOrDefaultAsync(x => x.UserName == userName);

        ArgumentNullException.ThrowIfNull(user);

        var roles = user.GetUserRoleMappings();
        var permissions = user.GetUserPermissionMappings();

        var claims = new List<Claim>
        {
            new(JwtClaimTypes.Name, user.UserName ?? string.Empty),
            new(JwtClaimTypes.GivenName, user.FirstName ?? string.Empty),
            new(JwtClaimTypes.FamilyName, user.LastName ?? string.Empty),
            new(JwtClaimTypes.Email, user.Email ?? string.Empty),
            new(JwtClaimTypes.Role, JsonConvert.SerializeObject(roles)),
            new("Permissions", JsonConvert.SerializeObject(permissions)),
            new("employee_id", user.EmployeeId ?? string.Empty),
            new("department", user.Department ?? string.Empty),
            new("job_title", user.JobTitle ?? string.Empty)
        };

        return (await _userManager.AddClaimsAsync(user, claims)).Succeeded;
    }

    public async Task<bool> AddToRoleAsync(ManagementUser user, string role)
    {
        return (await _userManager.AddToRoleAsync(user, role)).Succeeded;
    }

    public async Task<bool> AddToRolesAsync(ManagementUser user, List<string> roles)
    {
        return (await _userManager.AddToRolesAsync(user, roles)).Succeeded;
    }

    public async Task<bool> CreateUserAsync(ManagementUser user, string password)
    {
        return (await _userManager.CreateAsync(user, password)).Succeeded;
    }

    public async Task<bool> EmailExistsAsync(string email)
    {
        return await _userManager.FindByEmailAsync(email) is not null;
    }

    public async Task<string> GetEmailConfirmationToken(ManagementUser user)
    {
        return await _userManager.GenerateEmailConfirmationTokenAsync(user);
    }

    public async Task<bool> IsInRole(ManagementUser user, string role)
    {
        return await _userManager.IsInRoleAsync(user, role);
    }

    public async Task RemoveFromRoleAsync(ManagementUser user, string role)
    {
        await _userManager.RemoveFromRoleAsync(user, role);
    }

    public async Task<bool> UpdateUser(ManagementUser user)
    {
        return (await _userManager.UpdateAsync(user)).Succeeded;
    }

    public async Task<ManagementUser?> FindByEmailAsync(string email)
    {
        return await _userManager.Users
        .Include("UserRoles.Role.RolePermissions.Permission")
        .FirstOrDefaultAsync(x => x.Email == email);
    }

    public async Task<ManagementUser?> FindByIdAsync(string id)
    {
        return await _userManager.Users.Include("UserRoles.Role.RolePermissions.Permission").FirstOrDefaultAsync(x => x.Id == id);
    }

    public IQueryable<ManagementUser> AsQueryable()
    {
        return _userManager.Users.AsNoTracking().Include("UserRoles.Role.RolePermissions.Permission");
    }
}
