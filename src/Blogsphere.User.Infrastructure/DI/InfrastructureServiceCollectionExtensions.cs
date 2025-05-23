using Blogsphere.User.Domain.Entities;
using Blogsphere.User.Infrastructure.Database;
using Blogsphere.User.Infrastructure.HealthChecks;
using Blogsphere.User.Infrastructure.Security;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Blogsphere.User.Infrastructure.DI;
public static class InfrastructureServiceCollectionExtensions
{
    public static IServiceCollection ConfigureInfraServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<UserDbContext>(options =>
        {
            options.UseSqlServer(configuration.GetConnectionString("DefaultConnection"));
        });

        services.AddDbContext<DataProtectionKeyContext>(options =>
        {
            options.UseSqlServer(configuration.GetConnectionString("DefaultConnection"));
        });

        var appRootPath = Directory.GetCurrentDirectory();

        services.AddDataProtection()
        .PersistKeysToDbContext<DataProtectionKeyContext>()
        .SetApplicationName("blogsphere");

        services.AddHealthChecks()
            .AddCheck<DbHealthCheck>("sqlserver-health");

        // services.AddDataProtection()
        //     .PersistKeysToFileSystem(new DirectoryInfo(Path.Combine(appRootPath, "keys")))
        //     .SetApplicationName("blogsphere");

        services.AddIdentity<ApplicationUser, ApplicationRole>(options =>
        {
            options.SignIn.RequireConfirmedEmail = true;
        })
        .AddEntityFrameworkStores<UserDbContext>()
        .AddDefaultTokenProviders()
        .AddTokenProvider<ConfirmationEmailTokenProvider<ApplicationUser>>("EmailConfirmationTokenProvider");


        // masstransit service addition - using rabbitmq

        return services;
    }
}
