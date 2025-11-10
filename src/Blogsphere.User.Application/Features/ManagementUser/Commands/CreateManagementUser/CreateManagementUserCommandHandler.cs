using AutoMapper;
using Blogsphere.User.Application.Contracts.ActivityTracker;
using Blogsphere.User.Application.Contracts.Cache;
using Blogsphere.User.Application.Contracts.CQRS;
using Blogsphere.User.Application.Contracts.Data.Repositories;
using Blogsphere.User.Application.Contracts.Factory;
using Blogsphere.User.Application.Extensions;
using Blogsphere.User.Domain.Configurations;
using Blogsphere.User.Domain.Models.Constants;
using Blogsphere.User.Domain.Models.Core;
using Blogsphere.User.Domain.Models.Enums;
using Blogsphere.User.Domain.Models.Responses;
using Contracts.Events;
using Microsoft.Extensions.Options;

namespace Blogsphere.User.Application.Features.ManagementUser.Commands.CreateManagementUser;

public class CreateManagementUserCommandHandler(
    ILogger logger,
    IMapper mapper,
    IActivityTracker activityTracker,
    IManagementUserRepository managementUserRepository,
    ICacheServiceFactory cacheServiceFactory,
    IPublishServiceFactory publishServiceFactory,
    IOptions<AppConfigOption> appConfigOptions
) : ICommandHandler<CreateManagementUserCommand, Result<ManagementUserResponse>>
{
    private readonly ILogger _logger = logger;
    private readonly IMapper _mapper = mapper;
    private readonly IActivityTracker _activityTracker = activityTracker;
    private readonly IPublishServiceFactory _publishServiceFactory = publishServiceFactory;
    private readonly IManagementUserRepository _managementUserRepository = managementUserRepository;
    private readonly AppConfigOption _appConfigOption = appConfigOptions.Value;
    private readonly ICacheService _cacheService = cacheServiceFactory.Create(CacheServiceTypes.Distributed);

    public async Task<Result<ManagementUserResponse>> Handle(CreateManagementUserCommand request, CancellationToken cancellationToken)
    {
        _logger.Here().MethodEntered();
        _logger.Here().Information("Command executing {name}", nameof(CreateManagementUserCommand));

        var activity = _activityTracker.TrackCommandActivity(nameof(CreateManagementUserCommand), ("request", request));
        
        if(await IsEmailTaken(request.CreateManagementUserRequest.Email))
        {
            _logger.Here().Information("Email {email} already exists", request.CreateManagementUserRequest.Email);
            activity.SetTag(TrackerConstants.CommandStatus, "EmailAlreadyExists");
            return Result<ManagementUserResponse>.Failure(ErrorCodes.BadRequest, "Email already exists");
        }
        var managementUser = _mapper.Map<Domain.Entities.Management.ManagementUser>(request.CreateManagementUserRequest);
        UpdateProfileMetadata(managementUser, request.RequestInformation.CurrentUser.Id);
         
        var registrationResult = await _managementUserRepository.CreateUserAsync(managementUser, "Welcome@123");
        if (!registrationResult)
        {
            _logger.Here().Error("Failed to create management user");
            activity.SetTag(TrackerConstants.CommandStatus, "UserCreationFailed");
            return Result<ManagementUserResponse>.Failure(ErrorCodes.OperationFailed);
        }

        if (!await _managementUserRepository.AddToRolesAsync(managementUser, request.CreateManagementUserRequest.Roles))
        {
            _logger.Here().Error("Failed to assign roles to management user");
            activity.SetTag(TrackerConstants.CommandStatus, "RoleAssignmentFailed");
            return Result<ManagementUserResponse>.Failure(ErrorCodes.OperationFailed);
        }

        if (!await _managementUserRepository.AddToClaimsAsync(managementUser.Email))
        {
            _logger.Here().Error("Failed to assign claims to management user");
            activity.SetTag(TrackerConstants.CommandStatus, "ClaimsAssignmentFailed");
            return Result<ManagementUserResponse>.Failure(ErrorCodes.OperationFailed);
        }

        await PublishEvents(managementUser, request.RequestInformation);

        activity.SetTag(TrackerConstants.CommandStatus, "Success");
        _logger.Here().Information("Management user created successfully");
        _logger.Here().MethodExited();
        return Result<ManagementUserResponse>.Success(_mapper.Map<ManagementUserResponse>(managementUser));
    }

    private async Task PublishEvents(Domain.Entities.Management.ManagementUser managementUser, RequestInformation requestInformation)
    {
        // Publish management user created event
        var managementUserCreatedPublishService = _publishServiceFactory.CreatePublishService<Domain.Entities.Management.ManagementUser, ManagementUserCreated>();
        await managementUserCreatedPublishService.PublishAsync(managementUser, requestInformation.CorrelationId);

        // Publish welcome email
        if (_appConfigOption.ManagementUserWelcomeEmailEnabled)
        {
            var welcomeEmailPublishService = _publishServiceFactory.CreatePublishService<Domain.Entities.Management.ManagementUser, ManagementUserWelcomeEmailSent>();
            await welcomeEmailPublishService.PublishAsync(managementUser, requestInformation.CorrelationId);
        }
        
        // Publish password email
        if (_appConfigOption.ManagementUserPasswordEmailEnabled)
        {
            var passwordEmailPublishService = _publishServiceFactory.CreatePublishService<Domain.Entities.Management.ManagementUser, ManagementUserPasswordEmailSent>();
            await passwordEmailPublishService.PublishAsync(managementUser, requestInformation.CorrelationId);
        }
    }

    private static void UpdateProfileMetadata(Domain.Entities.Management.ManagementUser managementUser, string userId)
    {
        managementUser.SetCreatedBy(userId);
        managementUser.SetUpdatedBy(userId);
        managementUser.SetUpdationTime();
        managementUser.UpdateActiveStatus();
        managementUser.MarkEmailConfirmation();
        managementUser.MarkPhoneConfirmation();
        managementUser.MarkTwoFactorEnabled();
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

        exists = await _managementUserRepository.EmailExistsAsync(email);
        await _cacheService.SetAsync(cacheKey, false);

        return exists;
    }
}
