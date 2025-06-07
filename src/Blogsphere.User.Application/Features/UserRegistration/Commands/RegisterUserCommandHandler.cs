using Blogsphere.User.Application.Contracts.CQRS;
using Blogsphere.User.Application.Contracts.Data.Repositories;
using Blogsphere.User.Domain.Models.Core;
using Blogsphere.User.Domain.Models.Responses;

namespace Blogsphere.User.Application.Features.UserRegistration.Commands;

public class RegisterUserCommandHandler : ICommandHandler<RegisterUserCommand, Result<UserResponse>>
{
    private readonly ILogger _logger;
    private readonly IUserRepository _userRepository;

    public async Task<Result<UserResponse>> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}