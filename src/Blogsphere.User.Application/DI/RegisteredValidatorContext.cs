using Blogsphere.User.Application.Validators;
using Blogsphere.User.Domain.Models.Requests.Registration;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;

namespace Blogsphere.User.Application.DI;

public static class RegisteredValidatorContext
{
    public static IServiceCollection AddValidators(this IServiceCollection services)
    {
        services.AddScoped<IValidator<RegistrationRequest>, RegistrationRequestValidator>();
        return services;
    }
}
