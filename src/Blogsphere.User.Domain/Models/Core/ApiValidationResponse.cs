using Blogsphere.User.Domain.Models.Enums;

namespace Blogsphere.User.Domain.Models.Core;

public class ApiValidationResponse : ApiResponse
{
    public ApiValidationResponse(string errorMessage = "") : base(ErrorCodes.BadRequest, errorMessage)
    {
        ErrorMessage = !string.IsNullOrEmpty(errorMessage) ? errorMessage : GetDefaultErrorMessage(Code);
    }

    public List<FieldLevelError> Errors { get; set; }
    protected override string GetDefaultErrorMessage(ErrorCodes code)
    {
        return "Invalid data provided";
    }
}
