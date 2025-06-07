using Blogsphere.User.Domain.Models.Core;
using Swashbuckle.AspNetCore.Filters;

namespace Blogsphere.Swagger.Examples;

public class ValidationResponseExample : IExamplesProvider<ApiValidationResponse>
{
    public ApiValidationResponse GetExamples()
    {
        return new ApiValidationResponse("Invalid data provided"){
            Errors = [  
                new FieldLevelError
                {
                    Code = "Invalid",
                    Field = "Name",
                    Message = "Name is required"
                }
            ]
        };
    }
}
