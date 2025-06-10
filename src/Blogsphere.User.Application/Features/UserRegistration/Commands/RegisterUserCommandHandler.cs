using AutoMapper;
using Blogsphere.User.Application.Contracts.ActivityTracker;
using Blogsphere.User.Application.Contracts.Cache;
using Blogsphere.User.Application.Contracts.CQRS;
using Blogsphere.User.Application.Contracts.Data.Repositories;
using Blogsphere.User.Application.Contracts.Factory;
using Blogsphere.User.Application.Extensions;
using Blogsphere.User.Domain.Entities;
using Blogsphere.User.Domain.Models.Constants;
using Blogsphere.User.Domain.Models.Core;
using Blogsphere.User.Domain.Models.Enums;
using Blogsphere.User.Domain.Models.Responses;

namespace Blogsphere.User.Application.Features.UserRegistration.Commands;

public class RegisterUserCommandHandler(ILogger logger,
    IUserRepository userRepository,
    ICacheServiceFactory cacheServiceFactory,
    IActivityTracker activityTracker,
    IMapper mapper
) : ICommandHandler<RegisterUserCommand, Result<UserResponse>>
{
    private readonly IMapper _mapper = mapper;
    private readonly ILogger _logger = logger;
    private readonly IUserRepository _userRepository = userRepository;
    private readonly IActivityTracker _activityTracker = activityTracker;
    private readonly ICacheService _cacheService = cacheServiceFactory.Create(CacheServiceTypes.Distributed);

    public async Task<Result<UserResponse>> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
    {
        _logger.Here().MethodEntered();
        _logger.Here().Information("Command executing {name}", nameof(RegisterUserCommand));

        using var activity = _activityTracker.TrackCommandActivity(nameof(RegisterUserCommand), ("user.email", request.RegistrationRequest.Email));

        if (await IsEmailTaken(request.RegistrationRequest.Email))
        {
            _logger.Here().Error("Email {email} is already taken", request.RegistrationRequest.Email);
            activity?.SetTag(TrackerConstants.CommandStatus, "Failed");
            return Result<UserResponse>.Failure(ErrorCodes.BadRequest, "Email is already taken");
        }

        var applicationUser = _mapper.Map<ApplicationUser>(request.RegistrationRequest);
        applicationUser.SetCreatedBy("Registration channel");

        if (!await _userRepository.CreateUserAsync(applicationUser, request.RegistrationRequest.Password))
        {
            _logger.Here().Error("Failed to create new user {@username}", request.RegistrationRequest.Email);
            activity?.SetTag(TrackerConstants.CommandStatus, "Failed");
            return Result<UserResponse>.Failure(ErrorCodes.OperationFailed);
        }

        if (!await _userRepository.AddToRolesAsync(applicationUser, [request.RegistrationRequest.Role.ToString()]))
        {
            _logger.Here().Error("Failed to assign roles to {@username}", request.RegistrationRequest.Email);
            activity?.SetTag(TrackerConstants.CommandStatus, "Failed");
            return Result<UserResponse>.Failure(ErrorCodes.OperationFailed);
        }

        if (!await _userRepository.AddToClaimsAsync(request.RegistrationRequest.Email))
        {
            _logger.Here().Error("Failed to assign claims to {@username}", request.RegistrationRequest.Email);
            activity?.SetTag(TrackerConstants.CommandStatus, "Failed");
            return Result<UserResponse>.Failure(ErrorCodes.OperationFailed);
        }


        activity?.SetTag(TrackerConstants.CommandStatus, "Success");
        _logger.Here().Information("user {@username} created", request.RegistrationRequest.Email);
        _logger.Here().MethodExited();
        
        return Result<UserResponse>.Success(new() { Email = request.RegistrationRequest.Email });
    }

    private async Task<bool> IsEmailTaken(string email)
    {
        bool exists;
        var normalizedEmail = email.Trim().ToLowerInvariant();
        var cacheKey = $"email_exists:{normalizedEmail}";

        if (exists = await _cacheService.ContainsAsync(cacheKey))
        {
            _logger.Here().Information("Email check served from cache");
            return exists;
        }

        exists = await _userRepository.EmailExistsAsync(email);
        await _cacheService.SetAsync(cacheKey, false);

        return exists;
    }
}
