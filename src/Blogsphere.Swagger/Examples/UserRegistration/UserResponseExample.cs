using Blogsphere.User.Domain.Models.Responses;
using Swashbuckle.AspNetCore.Filters;

namespace Blogsphere.Swagger.Examples.UserRegistration;

public class UserResponseExample : IExamplesProvider<UserResponse>
{
    public UserResponse GetExamples()
    {
        return new UserResponse
        {
            Id = "123e4567-e89b-12d3-a456-426614174000",
            Email = "john.doe@example.com",
            Role = "Author"
        };
    }
}