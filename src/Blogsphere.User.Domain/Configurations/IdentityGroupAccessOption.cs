namespace Blogsphere.User.Domain.Configurations;

public class IdentityGroupAccessOption
{
    public const string OptionName = "IdentityGroupAccess";
    public string Authority { get; set; }
    public string Audience { get; set; }
}
