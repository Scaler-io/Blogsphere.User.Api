using Blogsphere.User.Domain.Attributes;
using FluentValidation;
using FluentValidation.Results;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace Blogsphere.User.Application.Behaviors;

public class ValidationBehavior<TRequest, TResponse>(IServiceProvider serviceProvider) : IPipelineBehavior<TRequest, TResponse>
{
    private readonly IServiceProvider _serviceProvider = serviceProvider;

    public async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken, RequestHandlerDelegate<TResponse> next)
    {
        List<ValidationFailure> validationFailures = [];

        // validate top-level request
        var requestValidator = _serviceProvider.GetService<IValidator<TRequest>>();
        
        if(requestValidator != null){
            var context = new ValidationContext<TRequest>(request);
            var validationResult = await requestValidator.ValidateAsync(context, cancellationToken);
            validationFailures.AddRange(validationResult.Errors);
        }

        // validate nested properties
        validationFailures.AddRange(ValidateNestedProperties(request, cancellationToken));

        if(validationFailures.Count != 0)
        {
            throw new ValidationException(validationFailures);
        }

        return await next();
    }

    private IEnumerable<ValidationFailure> ValidateNestedProperties(TRequest request, CancellationToken cancellationToken)
    {
        if(request == null)
        {
            return [];
        }

        var failures = new List<ValidationFailure>();
        var properties = request.GetType().GetProperties();

        foreach(var property in properties)
        {
            if(!property.IsDefined(typeof(ValidateNestedAttribute), inherit: true))
            {
                continue;
            }

            var propertyValue = property.GetValue(request);
            if(propertyValue == null)
            {
                continue;
            }

            var propertyType = property.PropertyType;
            var validatorType = typeof(IValidator<>).MakeGenericType(propertyType);
            var validator = _serviceProvider.GetService(validatorType);

            if(validator != null)
            {
                var context = new ValidationContext<object>(propertyValue);
                var validationResult = ((IValidator)validator).ValidateAsync(context, cancellationToken).Result;
                failures.AddRange(validationResult.Errors);
            }
        }
        
        return failures;
    }
}
