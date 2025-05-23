namespace Blogsphere.User.Domain.Entities;

public class ApplicationPermission(string name)
{
    public string Id { get; private init; } = Guid.NewGuid().ToString();
    public string Name { get; set; } = name;

    public ICollection<ApplicationRolePermission> RolePermissions { get; set; } = [];
}
