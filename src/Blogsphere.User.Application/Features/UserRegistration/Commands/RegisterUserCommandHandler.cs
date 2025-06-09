using Blogsphere.User.Application.Contracts.ActivityTracker;
using Blogsphere.User.Application.Contracts.Cache;
using Blogsphere.User.Application.Contracts.CQRS;
using Blogsphere.User.Application.Contracts.Data.Repositories;
using Blogsphere.User.Application.Contracts.Factory;
using Blogsphere.User.Application.Extensions;
using Blogsphere.User.Domain.Models.Constants;
using Blogsphere.User.Domain.Models.Core;
using Blogsphere.User.Domain.Models.Enums;
using Blogsphere.User.Domain.Models.Responses;

namespace Blogsphere.User.Application.Features.UserRegistration.Commands;

public class RegisterUserCommandHandler(ILogger logger,
    IUserRepository userRepository,
    ICacheServiceFactory cacheServiceFactory,
    IActivityTracker activityTracker
) : ICommandHandler<RegisterUserCommand, Result<UserResponse>>
{
    private readonly ILogger _logger = logger;
    private readonly IUserRepository _userRepository = userRepository;
    private readonly IActivityTracker _activityTracker = activityTracker;
    private readonly ICacheService _cacheService = cacheServiceFactory.Create(CacheServiceTypes.Distributed);

    public async Task<Result<UserResponse>> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
    {
        _logger.Here().MethodEntered();
        _logger.Here().Information("Command executing {name}", nameof(RegisterUserCommand));

        using var activity = _activityTracker.TrackCommandActivity(nameof(RegisterUserCommand), ("user.email", request.registrationRequest.Email));

        await _cacheService.SetAsync(key: "user_email", value: request.registrationRequest.Email, cancellation: cancellationToken);


        activity?.SetTag(TrackerConstants.CommandStatus, "Success");
        return Result<UserResponse>.Success(new() { Email = request.registrationRequest.Email });
    }
}