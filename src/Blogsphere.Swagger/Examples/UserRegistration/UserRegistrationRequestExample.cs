using Blogsphere.User.Domain.Models.Enums;
using Blogsphere.User.Domain.Models.Requests.Registration;
using Swashbuckle.AspNetCore.Filters;

namespace Blogsphere.Swagger.Examples.UserRegistration;

public class UserRegistrationRequestExample : IExamplesProvider<RegistrationRequest>
{
    public RegistrationRequest GetExamples()
    {
        return new RegistrationRequest
        {
            FirstName = "John",
            LastName = "Doe",
            Email = "john.doe@example.com",
            Password = "Password123!",
            ConfirmPassword = "Password123!",
            Role = Roles.Author,
            Bio = "I am a software engineer with a passion for building scalable and efficient systems.",
            WebsiteUrl = "https://www.example.com",
            LinkedIn = "https://www.linkedin.com/in/john-doe",
            Twitter = "https://www.twitter.com/john-doe",
            Instagram = "https://www.instagram.com/john-doe"

        };
    }
}
