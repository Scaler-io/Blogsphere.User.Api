using Blogsphere.User.Application.Contracts.Data.Repositories;
using Blogsphere.User.Application.Contracts.Factory;
using Blogsphere.User.Domain.Entities;
using Blogsphere.User.Infrastructure.Cache;
using Blogsphere.User.Infrastructure.Database;
using Blogsphere.User.Infrastructure.Database.Repositories;
using Blogsphere.User.Infrastructure.Factory;
using Blogsphere.User.Infrastructure.HealthChecks;
using Blogsphere.User.Infrastructure.Security;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using StackExchange.Redis;

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

        // for in-memory cache
        services.AddMemoryCache();

        // for redis
        services.AddStackExchangeRedisCache(options =>
        {
            options.InstanceName = configuration["Redis:InstanceName"];
            options.ConnectionMultiplexerFactory = async () =>
            {
                var config = configuration.GetConnectionString("Redis");
                var multiplexer = await ConnectionMultiplexer.ConnectAsync(config);
                return multiplexer;
            };
        });

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

        services.AddTransient<Application.Contracts.Data.IDbTransaction, DbTransaction>();
        services.AddScoped<IUserRepository, UserRepository>();

        // Caching
        services.AddScoped<ICacheServiceFactory, CacheServiceFactory>();
        services.AddScoped<InMemoryCacheService>();
        services.AddScoped<DistributedCacheService>();

        // masstransit service addition - using rabbitmq

        return services;
    }
}
