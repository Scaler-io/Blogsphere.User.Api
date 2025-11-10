using Blogsphere.User.Domain.Models.Responses;
using Swashbuckle.AspNetCore.Filters;

namespace Blogsphere.Swagger.Examples.ManagementUser;

public class ManagementUseResponseExample : IExamplesProvider<ManagementUserResponse>
{
    public ManagementUserResponse GetExamples()
    {
        return new ManagementUserResponse
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
        };
    }
}
