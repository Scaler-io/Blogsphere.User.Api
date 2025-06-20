using Blogsphere.User.Application.Contracts.Data.Repositories;
using Blogsphere.User.Application.Contracts.EventBus;
using Blogsphere.User.Application.Contracts.Factory;
using Blogsphere.User.Domain.Configurations;
using Blogsphere.User.Domain.Entities;
using Blogsphere.User.Infrastructure.Cache;
using Blogsphere.User.Infrastructure.Database;
using Blogsphere.User.Infrastructure.Database.Repositories;
using Blogsphere.User.Infrastructure.EventBus;
using Blogsphere.User.Infrastructure.Factory;
using Blogsphere.User.Infrastructure.HealthChecks;
using Blogsphere.User.Infrastructure.Security;
using MassTransit;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RabbitMQ.Client;

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
            .AddCheck<DbHealthCheck>("sqlserver-health")
            .AddCheck<RedisHealthCheck>("redis-health")
            .AddRabbitMQ(factory: _ => {
                var eventBus = configuration.GetSection(EventBusOption.OptionName).Get<EventBusOption>();
                var connectionFactory = new ConnectionFactory {
                    HostName = eventBus.Host,
                    UserName = eventBus.Username,
                    Password = eventBus.Password,
                    VirtualHost = eventBus.VirtualHost,
                    Port = eventBus.Port,
                    RequestedHeartbeat = TimeSpan.FromSeconds(60),
                    AutomaticRecoveryEnabled = true,
                };
                return connectionFactory.CreateConnectionAsync();
            }, "rabbitmq-health");

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
        services.AddScoped(typeof(IPublishService<,>), typeof(PublishService<,>));
        services.AddScoped<IPublishServiceFactory, PublishServiceFactory>();
        services.AddMassTransit(config => 
        {
            config.SetEndpointNameFormatter(new KebabCaseEndpointNameFormatter("users", false));
            config.UsingRabbitMq((context, cfg) => 
            {
                var eventBus = configuration.GetSection(EventBusOption.OptionName).Get<EventBusOption>();
                cfg.Host(eventBus.Host, eventBus.VirtualHost, host => 
                {
                    host.Username(eventBus.Username);
                    host.Password(eventBus.Password);
                });

                cfg.ConfigureEndpoints(context);
            });
        });
        
        return services;
    }
}
