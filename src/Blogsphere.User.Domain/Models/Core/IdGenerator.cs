using Blogsphere.User.Domain.Models.Enums.Management;

namespace Blogsphere.User.Application.Helpers;

public static class IdGenerator
{
    public const int MaxManagementEmployeeIdLength = 20;

    private static readonly Dictionary<string, string> ManagementRoleAbbreviations = new(StringComparer.OrdinalIgnoreCase)
    {
        [nameof(ManagementRoles.SuperAdmin)] = "SAD",
        [nameof(ManagementRoles.Admin)] = "ADM",
        [nameof(ManagementRoles.Manager)] = "MGR",
        [nameof(ManagementRoles.Moderator)] = "MOD",
        [nameof(ManagementRoles.Analyst)] = "ANL",
        [nameof(ManagementRoles.Support)] = "SUP",
        ["SystemAdmin"] = "SYS",
    };

    private static readonly HashSet<string> KnownRoleAbbreviations = new(StringComparer.OrdinalIgnoreCase)
    {
        "SAD", "ADM", "MGR", "MOD", "ANL", "SUP", "SYS"
    };

    public static string NewId() => Guid.NewGuid().ToString();

    public static string NewId(string prefix) => $"{prefix}_{Guid.NewGuid():N}";

    public static string NewShortId() => Guid.NewGuid().ToString("N")[..8].ToUpper();

    public static string NormalizeDepartment(string department)
    {
        if (string.IsNullOrWhiteSpace(department))
        {
            throw new ArgumentException("Department is required.", nameof(department));
        }

        return department.Trim().ToUpperInvariant();
    }

    public static string ResolveRoleAbbreviation(string role)
    {
        if (string.IsNullOrWhiteSpace(role))
        {
            throw new ArgumentException("Role is required.", nameof(role));
        }

        var trimmed = role.Trim();

        if (ManagementRoleAbbreviations.TryGetValue(trimmed, out var mappedAbbreviation))
        {
            return mappedAbbreviation;
        }

        var upper = trimmed.ToUpperInvariant();
        if (KnownRoleAbbreviations.Contains(upper))
        {
            return upper;
        }

        if (upper.Length == 3 && upper.All(char.IsLetter))
        {
            return upper;
        }

        var letters = trimmed.Where(char.IsLetter).Take(3).Select(char.ToUpperInvariant).ToArray();
        return letters.Length >= 3
            ? new string(letters, 0, 3)
            : new string(letters).PadRight(3, 'X');
    }

    public static string NewManagementUserId(string department, string role)
    {
        var dept = NormalizeDepartment(department);
        var roleAbbr = ResolveRoleAbbreviation(role);
        var suffix = NewShortId()[..4];
        var id = $"{dept}{roleAbbr}{DateTime.UtcNow:yyMMdd}{suffix}";

        if (id.Length > MaxManagementEmployeeIdLength)
        {
            throw new InvalidOperationException(
                $"Employee ID '{id}' exceeds {MaxManagementEmployeeIdLength} characters. Use shorter department/role codes.");
        }

        return id;
    }
}
