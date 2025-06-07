using Blogsphere.User.Domain.Models.Constants;
using Blogsphere.User.Domain.Models.Core;
using Blogsphere.User.Domain.Models.Enums;
using Swashbuckle.AspNetCore.Filters;

namespace Blogsphere.Swagger.Examples;

public class NotFoundResponseExample : IExamplesProvider<ApiResponse>
{
    public ApiResponse GetExamples()
    {
        return new(ErrorCodes.NotFound, ErrorMessages.NotFound);
    }
}