using Blogsphere.User.Domain.Models.Enums;

namespace Blogsphere.User.Domain.Models.Core;

public class Result<T>
{
    public T Data { get; set; }
    public bool IsSuccess { get; set; }
    public ErrorCodes ErrorCode { get; set; }
    public string ErrorMessage { get; set; }

    public static Result<T> Success(T data)
    {
        return new() {IsSuccess = true, Data = data};
    }

    public static Result<T> Failure(ErrorCodes errorCode, string errorMessage = "")
    {
        return new() { IsSuccess = false, ErrorCode = errorCode, ErrorMessage  = errorMessage};
    }
}
