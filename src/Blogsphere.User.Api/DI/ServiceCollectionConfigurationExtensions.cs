using Blogsphere.User.Domain.Configurations;

namespace Blogsphere.User.Api.DI;

public static class ServiceCollectionConfigurationExtensions
{
    public static IServiceCollection ConfigurationApplicationOptions(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<AppConfigOption>(configuration.GetSection(AppConfigOption.OptionName))
            .Configure<ElasticSearchOption>(configuration.GetSection(ElasticSearchOption.OptionName))
            .Configure<LoggingOption>(configuration.GetSection(LoggingOption.OptionName));         

        return services;
    }
}
