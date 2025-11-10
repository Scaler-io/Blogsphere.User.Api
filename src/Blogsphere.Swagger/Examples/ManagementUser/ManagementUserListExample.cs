using Blogsphere.User.Domain.Models.Core;
using Blogsphere.User.Domain.Models.Responses;
using Swashbuckle.AspNetCore.Filters;

namespace Blogsphere.Swagger.Examples.ManagementUser;

public class ManagementUserListExample : IExamplesProvider<PaginatedResult<ManagementUserResponse>>
{
    public PaginatedResult<ManagementUserResponse> GetExamples()
    {
        return new PaginatedResult<ManagementUserResponse>
        {
            Items = [
                new ManagementUserResponse
                {
                    Id = "1",
                    FirstName = "John",
                    LastName = "Doe",
                    UserName = "john.doe",
                    FullName = "John Doe",
                    IsActive = true,
                    IsEmailConfirmed = true,
                    IsPhoneNumberConfirmed = true,
                    IsTwoFactorEnabled = false,
                    DisplayName = "John Doe",
                    Email = "john.doe@example.com",
                    Department = "IT",
                    JobTitle = "Software Engineer",
                    EmployeeId = "1234567890",
                    Roles = [new RoleResponse { Name = "Admin", Description = "Admin role" }],
                    Metadata = new Metadata
                    {
                        CreatedAt = DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss"),
                        UpdatedAt = DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss"),
                        LastLoginAt = DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss"),
                        CreatedBy = "John Doe"  ,
                        UpdatedBy = "John Doe"
                    }
                },
                new ManagementUserResponse
                {
                    Id = "2",
                    FirstName = "Jane",
                    LastName = "Doe",
                    UserName = "jane.doe",
                    FullName = "Jane Doe",
                    IsActive = true,
                    IsEmailConfirmed = true,
                    IsPhoneNumberConfirmed = true,
                    IsTwoFactorEnabled = false,
                    DisplayName = "Jane Doe",
                    Email = "jane.doe@example.com",
                    Department = "IT",
                    JobTitle = "Software Engineer",
                    EmployeeId = "1234567890",
                    Roles = [new RoleResponse { Name = "Admin", Description = "Admin role" }],
                    Metadata = new Metadata
                    {
                        CreatedAt = DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss"),
                        UpdatedAt = DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss"),
                        LastLoginAt = DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss"),
                        CreatedBy = "Jane Doe",
                        UpdatedBy = "Jane Doe"
                    }
                }
            ],
            TotalCount = 2,
            PageNumber = 1,
            PageSize = 10
        };
    }
}
