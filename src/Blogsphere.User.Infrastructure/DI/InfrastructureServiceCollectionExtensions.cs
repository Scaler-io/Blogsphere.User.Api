using Blogsphere.User.Application.Contracts.Data.Repositories;
using Blogsphere.User.Application.Contracts.EventBus;
using Blogsphere.User.Application.Contracts.Factory;
using Blogsphere.User.Domain.Configurations;
using Blogsphere.User.Domain.Entities;
using Blogsphere.User.Domain.Entities.Management;
using Blogsphere.User.Domain.Models.Constants;
using Blogsphere.User.Infrastructure.Cache;
using Blogsphere.User.Infrastructure.Database;
using Blogsphere.User.Infrastructure.Database.Repositories;
using Blogsphere.User.Infrastructure.Database.Repositories.Management;
using Blogsphere.User.Infrastructure.EventBus;
using Blogsphere.User.Infrastructure.Factory;
using Blogsphere.User.Infrastructure.HealthChecks;
using Blogsphere.User.Infrastructure.Security;
using Blogsphere.User.Infrastructure.Security.ManagementUser;
using MassTransit;
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

        services.AddDbContext<ManagementUserDbContext>(options =>
        {
            options.UseSqlServer(configuration.GetConnectionString("DefaultConnection"), 
                sqlOptions => sqlOptions.MigrationsHistoryTable("__EFMigrationsHistory", "Management"));
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
            .AddCheck<DbHealthCheck>("sqlserver-health")
            .AddCheck<RedisHealthCheck>("redis-health");

        // for in-memory cache
        services.AddMemoryCache();

        // for redis
        services.AddStackExchangeRedisCache(options =>
        {
            options.InstanceName = configuration["Redis:InstanceName"];
            options.Configuration = configuration.GetConnectionString("Redis");
        });

        // services.AddDataProtection()
        //     .PersistKeysToFileSystem(new DirectoryInfo(Path.Combine(appRootPath, "keys")))
        //     .SetApplicationName("blogsphere");

        services.AddIdentityCore<ApplicationUser>(options =>
        {
            // Password requirements
            options.Password.RequireDigit = true;
            options.Password.RequiredLength = 8;
            options.Password.RequireNonAlphanumeric = true;
            options.Password.RequireUppercase = true;
            options.Password.RequireLowercase = true;
            options.Tokens.EmailConfirmationTokenProvider = "EmailConfirmationTokenProvider";
        })
        .AddRoles<ApplicationRole>()
        .AddEntityFrameworkStores<UserDbContext>()
        .AddDefaultTokenProviders()
        .AddTokenProvider<ConfirmationEmailTokenProvider<ApplicationUser>>("EmailConfirmationTokenProvider");

        services.AddIdentityCore<ManagementUser>(options =>
        {
            // Password requirements
            options.Password.RequireDigit = true;
            options.Password.RequiredLength = 8;
            options.Password.RequireNonAlphanumeric = true;
            options.Password.RequireUppercase = true;
            options.Password.RequireLowercase = true;
    
            options.Tokens.EmailConfirmationTokenProvider = TokenOptions.DefaultEmailProvider;
        })
        .AddRoles<ManagementRole>()
        .AddEntityFrameworkStores<ManagementUserDbContext>()
        .AddDefaultTokenProviders()
        .AddTokenProvider<ManagementEmailConfirmationTokenProvider<ManagementUser>>(ManagementConstants.ManagementEmailTokenProvider)
        .AddClaimsPrincipalFactory<UserClaimsPrincipalFactory<ManagementUser>>();

        services.AddTransient<Application.Contracts.Data.IDbTransaction, DbTransaction>();
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IManagementUserRepository, ManagementUserRepository>();

        // Caching
        services.AddScoped<ICacheServiceFactory, CacheServiceFactory>();
        services.AddScoped<InMemoryCacheService>();
        services.AddScoped<DistributedCacheService>();

        // masstransit service addition - using rabbitmq
        services.AddScoped(typeof(IPublishService<,>), typeof(PublishService<,>));
        services.AddScoped<IPublishServiceFactory, PublishServiceFactory>();
        services.AddMassTransit(config => 
        {
            config.SetEndpointNameFormatter(new KebabCaseEndpointNameFormatter("users", false));
            config.UsingRabbitMq((context, cfg) => 
            {
                var eventBus = configuration.GetSection(EventBusOption.OptionName).Get<EventBusOption>() ?? throw new InvalidOperationException("Event bus configuration is not set");
                cfg.Host(eventBus.Host, eventBus.VirtualHost ?? "/", host => 
                {
                    host.Username(eventBus.Username);
                    host.Password(eventBus.Password);
                });
                cfg.UseMessageRetry(retry => retry.Interval(3, 1000));
                cfg.ConfigureEndpoints(context);
            });
        });
        
        return services;
    }
}
