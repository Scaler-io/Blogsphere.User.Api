using Blogsphere.User.Application.Behaviors;
using Blogsphere.User.Application.Contracts.ActivityTracker;
using Blogsphere.User.Application.Contracts.Security;
using Blogsphere.User.Application.Security;
using Blogsphere.User.Domain.Entities;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using System.Diagnostics;
using System.Reflection;

namespace Blogsphere.User.Application.DI;
public static class BusinessLogicServiceCollectionExtensions
{
    public static IServiceCollection ConfigureBusinessLogicServices(this IServiceCollection services)
    {

        services.AddMediatR(AppDomain.CurrentDomain.GetAssemblies());
        services.AddMediatR(Assembly.GetExecutingAssembly());
        services.AddValidators();

        services.AddSingleton<IPermissionMapper, PermissionMapper>(sp =>
        {
            using var scope = sp.CreateScope();
            var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<ApplicationRole>>();
            return new PermissionMapper(roleManager);
        });

        // auto mapper
        services.AddAutoMapper(Assembly.GetExecutingAssembly());

        services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());

        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>))
        .AddTransient(typeof(IPipelineBehavior<,>), typeof(DbTransactionBehavior<,>));


        // activity tracker
        services.AddSingleton(new ActivitySource("Blogsphere.User.API"));
        services.AddSingleton<IActivityTracker, ActivityTracker.ActivityTracker>();

        return services;
    }
}
