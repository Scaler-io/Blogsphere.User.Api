using Blogsphere.User.Application.Contracts.Data.Repositories;
using Blogsphere.User.Application.Extensions;
using Blogsphere.User.Domain.Entities;
using IdentityModel;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace Blogsphere.User.Infrastructure.Database.Repositories;

public class UserRepository(UserManager<ApplicationUser> userManager) : IUserRepository
{
    private readonly UserManager<ApplicationUser> _userManager = userManager;

    public async Task<bool> AddToClaimsAsync(string userName)
    {
        var user = await _userManager.Users
            .AsNoTracking()
            .Include("UserRoles.Role.RolePermissions.Permission")
            .FirstOrDefaultAsync(x => x.UserName == userName);

        ArgumentNullException.ThrowIfNull(user);

        var roles = user.GetUserRoleMappings();
        var permissions = user.GetUserPermissionMappings();

        return (await _userManager.AddClaimsAsync(user,
        [
            new(JwtClaimTypes.Name, user.UserName ?? string.Empty),
            new(JwtClaimTypes.GivenName, user.FirstName ?? string.Empty),
            new(JwtClaimTypes.FamilyName, user.Lastname ?? string.Empty),
            new(JwtClaimTypes.Email, user.Email ?? string.Empty),
            new(JwtClaimTypes.Role, JsonConvert.SerializeObject(roles)),
            new("Permissions", JsonConvert.SerializeObject(permissions))
        ])).Succeeded;
    }

    public async Task AddToRoleAsync(ApplicationUser user, string role)
    {
        await _userManager.AddToRoleAsync(user, role);
    }

    public async Task<bool> AddToRolesAsync(ApplicationUser user, IEnumerable<string> roles)
    {
        return (await _userManager.AddToRolesAsync(user, roles)).Succeeded;
    }

    public async Task<bool> CreateUserAsync(ApplicationUser user, string password)
    {
        return (await _userManager.CreateAsync(user, password)).Succeeded;
    }

    public async Task<bool> EmailExistsAsync(string email)
    {
        return await _userManager.FindByEmailAsync(email) is not null;
    }

    public async Task<string> GetEmailConfirmationToken(ApplicationUser user)
    {
        return await _userManager.GenerateEmailConfirmationTokenAsync(user);
    }

    public async Task<bool> IsInRole(ApplicationUser user, string role)
    {
        return await _userManager.IsInRoleAsync(user, role);
    }

    public async Task RemoveFromRoleAsync(ApplicationUser user, string role)
    {
        await _userManager.RemoveFromRoleAsync(user, role);
    }

    public async Task<bool> UpdateUser(ApplicationUser user)
    {
        return (await _userManager.UpdateAsync(user)).Succeeded;
    }
}
