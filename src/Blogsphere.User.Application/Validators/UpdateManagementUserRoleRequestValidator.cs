using Blogsphere.User.Domain.Models.Core;
using Blogsphere.User.Domain.Models.Requests.ManagementUser;
using FluentValidation;

namespace Blogsphere.User.Application.Validators;

public class UpdateManagementUserRoleRequestValidator : AbstractValidator<UpdateManagementUserRoleRequest>
{
    public UpdateManagementUserRoleRequestValidator()
    {
        RuleFor(x => x.Id)
        .NotEmpty()
        .WithErrorCode(ApiError.ManagementUserIdRequired().Code)
        .WithMessage(ApiError.ManagementUserIdRequired().ErrorMessages);
        
        RuleFor(x => x.Role)
        .NotEmpty()
        .WithErrorCode(ApiError.RolesRequired().Code)
        .WithMessage(ApiError.RolesRequired().ErrorMessages);
    }
}
