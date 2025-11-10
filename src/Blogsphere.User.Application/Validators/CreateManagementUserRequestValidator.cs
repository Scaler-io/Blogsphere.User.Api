using Blogsphere.User.Domain.Models.Core;
using Blogsphere.User.Domain.Models.Requests.ManagementUser;
using FluentValidation;

namespace Blogsphere.User.Application.Validators;

public class CreateManagementUserRequestValidator : AbstractValidator<CreateManagementUserRequest>
{
    public CreateManagementUserRequestValidator()
    {
        RuleFor(x => x.FirstName)
        .NotEmpty()
        .WithErrorCode(ApiError.FirstNameRequired().Code)
        .WithMessage(ApiError.FirstNameRequired().ErrorMessages);

        RuleFor(x => x.LastName)
        .NotEmpty()
        .WithErrorCode(ApiError.LastNameRequired().Code)
        .WithMessage(ApiError.LastNameRequired().ErrorMessages);

        RuleFor(x => x.Email)
        .NotEmpty()
        .WithErrorCode(ApiError.EmailRequired().Code)
        .WithMessage(ApiError.EmailRequired().ErrorMessages)
        .EmailAddress()
        .WithErrorCode(ApiError.InvalidEmail().Code)
        .WithMessage(ApiError.InvalidEmail().ErrorMessages);

        RuleFor(x => x.PhoneNumber)
        .NotEmpty()
        .WithErrorCode(ApiError.PhoneNumberRequired().Code)
        .WithMessage(ApiError.PhoneNumberRequired().ErrorMessages)
        .Matches(@"^\+?[1-9]\d{1,14}$")
        .WithErrorCode(ApiError.InvalidPhoneNumber().Code)
        .WithMessage(ApiError.InvalidPhoneNumber().ErrorMessages);
        
        RuleFor(x => x.Department)
        .NotEmpty()
        .WithErrorCode(ApiError.DepartmentRequired().Code)
        .WithMessage(ApiError.DepartmentRequired().ErrorMessages);

        RuleFor(x => x.JobTitle)
        .NotEmpty()
        .WithErrorCode(ApiError.JobTitleRequired().Code)
        .WithMessage(ApiError.JobTitleRequired().ErrorMessages);

        RuleFor(x => x.Roles)
        .NotEmpty()
        .WithErrorCode(ApiError.RolesRequired().Code)
        .WithMessage(ApiError.RolesRequired().ErrorMessages);
    }
}
