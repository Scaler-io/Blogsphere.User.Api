using Blogsphere.User.Domain.Models.Requests.ManagementUser;
using Swashbuckle.AspNetCore.Filters;

namespace Blogsphere.Swagger.Examples.ManagementUser;

public class UpdateManagementUserRoleRequestExample : IExamplesProvider<UpdateManagementUserRoleRequest>
{
    public UpdateManagementUserRoleRequest GetExamples()
    {
        return new UpdateManagementUserRoleRequest
        {
            Id = "1",
            Role = "Admin"
        };
    }
}
