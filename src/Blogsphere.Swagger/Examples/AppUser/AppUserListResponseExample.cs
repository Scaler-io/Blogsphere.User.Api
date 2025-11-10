using Blogsphere.User.Domain.Models.Core;
using Blogsphere.User.Domain.Models.Responses;
using Swashbuckle.AspNetCore.Filters;

namespace Blogsphere.Swagger.Examples.AppUser;

public class AppUserListResponseExample : IExamplesProvider<PaginatedResult<AppUserResponse>>
{
    public PaginatedResult<AppUserResponse> GetExamples()
    {
        return new PaginatedResult<AppUserResponse>
        {
            Items = [new AppUserResponse
            {
                Id = "1",
                FirstName = "John",
                LastName = "Doe",
                UserName = "john.doe",
                Email = "john.doe@example.com",
                PhoneNumber = "1234567890",
                IsActive = true,
                IsEmailConfirmed = true,
                IsPhoneNumberConfirmed = true,
                IsTwoFactorEnabled = false,
                ImageUrl = "https://example.com/image.jpg",
                Roles = [new RoleResponse { Name = "Admin", Description = "Admin role" }],
                Permissions = ["Read", "Write"],
                Profile = new ProfileResponse { Bio = "John Doe", WebsiteUrl = "https://example.com", LinkedIn = "https://example.com/linkedin", Twitter = "https://example.com/twitter", Instagram = "https://example.com/instagram" },
                Metadata = new Metadata { CreatedAt = DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss"), UpdatedAt = DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss"), LastLoginAt = DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss"), CreatedBy = "John Doe", UpdatedBy = "John Doe" }
            },
            new AppUserResponse
            {
                Id = "2",
                FirstName = "Jane",
                LastName = "Doe",
                UserName = "jane.doe",
                Email = "jane.doe@example.com",
                PhoneNumber = "1234567890",
                IsActive = true,
                IsEmailConfirmed = true,
                IsPhoneNumberConfirmed = true,
                IsTwoFactorEnabled = false,
                ImageUrl = "https://example.com/image.jpg",
                Roles = [new RoleResponse { Name = "Admin", Description = "Admin role" }],
                Permissions = ["Read", "Write"],
                Profile = new ProfileResponse { Bio = "Jane Doe", WebsiteUrl = "https://example.com", LinkedIn = "https://example.com/linkedin", Twitter = "https://example.com/twitter", Instagram = "https://example.com/instagram" },
                Metadata = new Metadata { CreatedAt = DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss"), UpdatedAt = DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss"), LastLoginAt = DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss"), CreatedBy = "Jane Doe", UpdatedBy = "Jane Doe" }
            }],
            TotalCount = 2,
            PageNumber = 1,
            PageSize = 10
        };
    }
    
}
