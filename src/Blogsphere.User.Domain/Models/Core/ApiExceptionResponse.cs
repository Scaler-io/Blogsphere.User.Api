using Blogsphere.User.Domain.Models.Enums;

namespace Blogsphere.User.Domain.Models.Core;

public class ApiExceptionResponse(string errorMessage = "", string stackTrace = "") : ApiResponse(ErrorCodes.InternalServerError, errorMessage)
{
    public string StackTrace { get; set; } = stackTrace;
}
