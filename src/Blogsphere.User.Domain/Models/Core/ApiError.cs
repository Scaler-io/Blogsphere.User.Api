namespace Blogsphere.User.Domain.Models.Core;

public class ApiError(string code, string errorMessages)
{
    public string Code { get; set; } = code;
    public string ErrorMessages { get; set; } = errorMessages;
}
