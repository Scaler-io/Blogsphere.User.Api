namespace Blogsphere.User.Application.Helpers;

public static class IdGenerator
{
    public static string NewId() => Guid.NewGuid().ToString();
    public static string NewId(string prefix) => $"{prefix}_{Guid.NewGuid():N}";
    public static string NewShortId() => Guid.NewGuid().ToString("N")[..8].ToUpper();
    public static string NewManagementUserId(string department, string role) 
        => $"{department}{role}{DateTime.UtcNow:yyMMdd}{NewShortId()[..4]}";
}
