namespace Blogsphere.User.Domain.Models.Core;

public sealed class ApiError(string code, string errorMessages)
{
    public string Code { get; set; } = code;
    public string ErrorMessages { get; set; } = errorMessages;

    public static ApiError BioTooShort() => new("BioTooShort", "Bio must be at least 10 characters long");
    public static ApiError BioTooLong() => new("BioTooLong", "Bio must be less than 500 characters long");

    public static ApiError ConfirmPasswordRequired() => new("ConfirmPasswordRequired", "Confirm password is required");
    public static ApiError PasswordsDoNotMatch() => new("PasswordsDoNotMatch", "Passwords do not match");

    public static ApiError EmailRequired() => new("EmailRequired", "Email is required");
    public static ApiError InvalidEmail() => new("InvalidEmail", "Invalid email address");

    public static ApiError FirstNameRequired() => new("FirstNameRequired", "First name is required");
    public static ApiError FirstNameTooShort() => new("FirstNameTooShort", "First name must be at least 3 characters long");
    public static ApiError FirstNameTooLong() => new("FirstNameTooLong", "First name must be less than 50 characters long");

    public static ApiError InvalidInstagramUrl() => new("InvalidInstagramUrl", "Invalid Instagram URL");

    public static ApiError LastNameRequired() => new("LastNameRequired", "Last name is required");
    public static ApiError LastNameTooShort() => new("LastNameTooShort", "Last name must be at least 3 characters long");
    public static ApiError LastNameTooLong() => new("LastNameTooLong", "Last name must be less than 50 characters long");

    public static ApiError InvalidLinkedInUrl() => new("InvalidLinkedInUrl", "Invalid LinkedIn URL");

    public static ApiError PasswordRequired() => new("PasswordRequired", "Password is required");
    public static ApiError PasswordTooShort() => new("PasswordTooShort", "Password must be at least 8 characters long");
    public static ApiError PasswordTooLong() => new("PasswordTooLong", "Password must be less than 50 characters long");

    public static ApiError InvalidTwitterUrl() => new("InvalidTwitterUrl", "Invalid Twitter URL");

    public static ApiError InvalidWebsiteUrl() => new("InvalidWebsiteUrl", "Invalid website URL");
}