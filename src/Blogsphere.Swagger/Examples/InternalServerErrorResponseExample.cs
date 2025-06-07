using Blogsphere.User.Domain.Models.Constants;
using Blogsphere.User.Domain.Models.Core;
using Swashbuckle.AspNetCore.Filters;

namespace Blogsphere.Swagger.Examples;

public class InternalServerErrorResponseExample : IExamplesProvider<ApiExceptionResponse>
{
    public ApiExceptionResponse GetExamples()
    {
        return new ApiExceptionResponse(ErrorMessages.InternalServerError, "Stack Trace");
    }
}