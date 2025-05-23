namespace Blogsphere.User.Domain.Configurations;

public class AppConfigOption
{
    public const string OptionName = "AppConfigurations";
    public string ApplicationIdentifier { get; set; }
    public string ApplicationEnvironment { get; set; }
    public int HealthCheckTimeoutInSeconds {  get; set; }
    public int CacheExpiration { get; set; }
    public string CategoryCacheKey {  get; set; }
    public string ProductCacheKey { get; set; }    
}
