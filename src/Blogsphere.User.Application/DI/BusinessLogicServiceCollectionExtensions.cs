using Blogsphere.User.Application.Behaviors;
using Blogsphere.User.Application.Contracts.Security;
using Blogsphere.User.Application.Security;
using Blogsphere.User.Domain.Entities;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace Blogsphere.User.Application.DI;
public static class BusinessLogicServiceCollectionExtensions
{
    public static IServiceCollection ConfigureBusinessLogicServices(this IServiceCollection services)
    {

        services.AddMediatR(Assembly.GetExecutingAssembly());

        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(DbTransactionBehavior<,>));

        services.AddSingleton<IPermissionMapper, PermissionMapper>(sp =>
        {
            using var scope = sp.CreateScope();
            var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<ApplicationRole>>();
            return new PermissionMapper(roleManager);
        });

        // auto mapper
        services.AddAutoMapper(Assembly.GetExecutingAssembly());

        // Fluent validation
        services.AddValidatorsFromAssemblies(AppDomain.CurrentDomain.GetAssemblies());

        return services;
    }
}
