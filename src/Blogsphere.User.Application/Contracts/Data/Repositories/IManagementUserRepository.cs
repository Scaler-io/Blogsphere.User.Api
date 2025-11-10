using Blogsphere.User.Domain.Entities.Management;

namespace Blogsphere.User.Application.Contracts.Data.Repositories;

public interface IManagementUserRepository
{
    Task<bool> CreateUserAsync(ManagementUser user, string password);
    Task<bool> AddToRoleAsync(ManagementUser user, string role);
    Task<bool> AddToRolesAsync(ManagementUser user, List<string> roles);
    Task RemoveFromRoleAsync(ManagementUser user, string role);
    Task<bool> IsInRole(ManagementUser user, string role);
    Task<bool> EmailExistsAsync(string email);
    Task<bool> UpdateUser(ManagementUser user);
    Task<bool> AddToClaimsAsync(string userName);
    Task<string> GetEmailConfirmationToken(ManagementUser user);
    Task<ManagementUser> FindByEmailAsync(string email);
    Task<ManagementUser> FindByIdAsync(string id);
    IQueryable<ManagementUser> AsQueryable();
}
