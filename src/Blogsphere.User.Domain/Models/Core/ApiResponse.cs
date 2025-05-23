using Blogsphere.User.Domain.Models.Constants;
using Blogsphere.User.Domain.Models.Enums;

namespace Blogsphere.User.Domain.Models.Core;

public class ApiResponse
{
    public ApiResponse(ErrorCodes code, string errorMessage = null)
    {
        Code = code;
        ErrorMessage = errorMessage ?? GetDefaultErrorMessage(code);
    }

    public ErrorCodes Code { get; set; }
    public string ErrorMessage { get; set; } 

    protected virtual string GetDefaultErrorMessage(ErrorCodes codes)
    {
        return codes switch 
        {
            ErrorCodes.BadRequest => ErrorMessages.BadRequest,
            ErrorCodes.NotFound => ErrorMessages.NotFound,
            ErrorCodes.Unauthorized => ErrorMessages.Unauthorized,
            ErrorCodes.OperationFailed => ErrorMessages.Operationfailed,
            ErrorCodes.InternalServerError => ErrorMessages.InternalServerError,
            ErrorCodes.NotAllowed => ErrorMessages.NotAllowed,
            _ => string.Empty,
        };
    }  
}
