using Blogsphere.User.Application.Contracts.CQRS;
using Blogsphere.User.Domain.Attributes;
using Blogsphere.User.Domain.Models.Core;
using Blogsphere.User.Domain.Models.Requests.Registration;
using Blogsphere.User.Domain.Models.Responses;

namespace Blogsphere.User.Application.Features.UserRegistration.Commands;

public class RegisterUserCommand(RegistrationRequest registrationRequest, RequestInformation requestInformation) 
    : ICommand<Result<UserResponse>>
{
    [ValidateNested]
    public RegistrationRequest RegistrationRequest { get; set; } = registrationRequest;
    public RequestInformation RequestInformation { get; set; } = requestInformation;
}

