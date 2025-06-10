using Blogsphere.User.Domain.Entities;

namespace Blogsphere.User.Application.Contracts.Data.Repositories;

public interface IUserRepository
{
    Task<bool> CreateUserAsync(ApplicationUser user, string password);
    Task<bool> AddToRoleAsync(ApplicationUser user, string role);
    Task<bool> AddToRolesAsync(ApplicationUser user, List<string> roles);
    Task RemoveFromRoleAsync(ApplicationUser user, string role);
    Task<bool> IsInRole(ApplicationUser user, string role);
    Task<bool> EmailExistsAsync(string email);
    Task<bool> UpdateUser(ApplicationUser user);
    Task<bool> AddToClaimsAsync(string userName);
    Task<string> GetEmailConfirmationToken(ApplicationUser user);
}

