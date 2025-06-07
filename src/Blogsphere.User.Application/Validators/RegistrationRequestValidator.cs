using Blogsphere.User.Domain.Models.Core;
using Blogsphere.User.Domain.Models.Enums;
using Blogsphere.User.Domain.Models.Requests.Registration;
using FluentValidation;

namespace Blogsphere.User.Application.Validators;

public class RegistrationRequestValidator : AbstractValidator<RegistrationRequest>
{
    [Obsolete]
    public RegistrationRequestValidator()
    {
        RuleFor(x => x.FirstName)
        .Cascade(CascadeMode.StopOnFirstFailure)
        .NotEmpty()
        .WithErrorCode(ApiError.FirstNameRequired().Code)
        .WithMessage(ApiError.FirstNameRequired().ErrorMessages)
        .MinimumLength(3)
        .WithErrorCode(ApiError.FirstNameTooShort().Code)
        .WithMessage(ApiError.FirstNameTooShort().ErrorMessages)
        .MaximumLength(50)
        .WithErrorCode(ApiError.FirstNameTooLong().Code)
        .WithMessage(ApiError.FirstNameTooLong().ErrorMessages);

        RuleFor(x => x.LastName)
                    .Cascade(CascadeMode.StopOnFirstFailure)
        .NotEmpty()
        .WithErrorCode(ApiError.LastNameRequired().Code)
        .WithMessage(ApiError.LastNameRequired().ErrorMessages)
        .MinimumLength(3)
        .WithErrorCode(ApiError.LastNameTooShort().Code)
        .WithMessage(ApiError.LastNameTooShort().ErrorMessages)
        .MaximumLength(50)
        .WithErrorCode(ApiError.LastNameTooLong().Code)
        .WithMessage(ApiError.LastNameTooLong().ErrorMessages);

        RuleFor(x => x.Email)
                    .Cascade(CascadeMode.StopOnFirstFailure)
        .NotEmpty()
        .WithErrorCode(ApiError.EmailRequired().Code)
        .WithMessage(ApiError.EmailRequired().ErrorMessages)
        .EmailAddress()
        .WithErrorCode(ApiError.InvalidEmail().Code)
        .WithMessage(ApiError.InvalidEmail().ErrorMessages);

        RuleFor(x => x.Password)
                    .Cascade(CascadeMode.StopOnFirstFailure)
        .NotEmpty()
        .WithErrorCode(ApiError.PasswordRequired().Code)
        .WithMessage(ApiError.PasswordRequired().ErrorMessages)
        .MinimumLength(8)
        .WithErrorCode(ApiError.PasswordTooShort().Code)
        .WithMessage(ApiError.PasswordTooShort().ErrorMessages)
        .MaximumLength(50)
        .WithErrorCode(ApiError.PasswordTooLong().Code)
        .WithMessage(ApiError.PasswordTooLong().ErrorMessages);

        RuleFor(x => x.ConfirmPassword)
                    .Cascade(CascadeMode.StopOnFirstFailure)
        .NotEmpty()
        .WithErrorCode(ApiError.ConfirmPasswordRequired().Code)
        .WithMessage(ApiError.ConfirmPasswordRequired().ErrorMessages)
        .Equal(x => x.Password)
        .WithErrorCode(ApiError.PasswordsDoNotMatch().Code)
        .WithMessage(ApiError.PasswordsDoNotMatch().ErrorMessages);

        RuleFor(x => x.Bio)
        .MinimumLength(10)
        .WithErrorCode(ApiError.BioTooShort().Code)
        .WithMessage(ApiError.BioTooShort().ErrorMessages)
        .When(x => !string.IsNullOrEmpty(x.Bio))
        .MaximumLength(500)
        .WithErrorCode(ApiError.BioTooLong().Code)
        .WithMessage(ApiError.BioTooLong().ErrorMessages)
        .When(x => !string.IsNullOrEmpty(x.Bio) && x.Role == Roles.Author);

        RuleFor(x => x.WebsiteUrl)
        .Must(uri => Uri.TryCreate(uri, UriKind.Absolute, out _))
        .WithErrorCode(ApiError.InvalidWebsiteUrl().Code)
        .WithMessage(ApiError.InvalidWebsiteUrl().ErrorMessages)
        .When(x => !string.IsNullOrEmpty(x.WebsiteUrl) && x.Role == Roles.Author);
        
        RuleFor(x => x.LinkedIn)
        .Must(uri => Uri.TryCreate(uri, UriKind.Absolute, out _))
        .WithErrorCode(ApiError.InvalidLinkedInUrl().Code)
        .WithMessage(ApiError.InvalidLinkedInUrl().ErrorMessages)
        .When(x => !string.IsNullOrEmpty(x.LinkedIn) && x.Role == Roles.Author);

        RuleFor(x => x.Twitter)
        .Must(uri => Uri.TryCreate(uri, UriKind.Absolute, out _))
        .WithErrorCode(ApiError.InvalidTwitterUrl().Code)
        .WithMessage(ApiError.InvalidTwitterUrl().ErrorMessages)
        .When(x => !string.IsNullOrEmpty(x.Twitter) && x.Role == Roles.Author);

        RuleFor(x => x.Instagram)
        .Must(uri => Uri.TryCreate(uri, UriKind.Absolute, out _))
        .WithErrorCode(ApiError.InvalidInstagramUrl().Code)
        .WithMessage(ApiError.InvalidInstagramUrl().ErrorMessages)
        .When(x => !string.IsNullOrEmpty(x.Instagram) && x.Role == Roles.Author);
    }
}
