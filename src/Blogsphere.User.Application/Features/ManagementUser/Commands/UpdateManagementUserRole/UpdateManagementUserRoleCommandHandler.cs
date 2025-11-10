using AutoMapper;
using Blogsphere.User.Application.Contracts.ActivityTracker;
using Blogsphere.User.Application.Contracts.CQRS;
using Blogsphere.User.Application.Contracts.Data.Repositories;
using Blogsphere.User.Application.Contracts.Factory;
using Blogsphere.User.Application.Extensions;
using Blogsphere.User.Application.Features.ManagementUser.Commands.UpdateManagementUserRole;
using Blogsphere.User.Domain.Models.Constants;
using Blogsphere.User.Domain.Models.Core;
using Blogsphere.User.Domain.Models.Enums;
using Blogsphere.User.Domain.Models.Responses;
using Contracts.Events;

namespace Blogsphere.User.Application.Features.ManagementUser.Commands.UpdateManagementUser;

public class UpdateManagementUserRoleCommandHandler(
    IManagementUserRepository managementUserRepository,
    ILogger logger,
    IActivityTracker activityTracker,
    IMapper mapper,
    IPublishServiceFactory publishServiceFactory) 
    : ICommandHandler<UpdateManagementUserRoleCommand, Result<ManagementUserResponse>>
{
    private readonly ILogger _logger = logger;
    private readonly IActivityTracker _activityTracker = activityTracker;
    private readonly IMapper _mapper = mapper;
    private readonly IManagementUserRepository _managementUserRepository = managementUserRepository;

    private readonly IPublishServiceFactory _publishServiceFactory = publishServiceFactory;
    
    public async Task<Result<ManagementUserResponse>> Handle(UpdateManagementUserRoleCommand request, CancellationToken cancellationToken)
    {   
        _logger.Here().MethodEntered();
        _logger.Here().Information("Command executing {name}", nameof(UpdateManagementUserRoleCommand));

        using var activity = _activityTracker.TrackCommandActivity(nameof(UpdateManagementUserRoleCommand), ("request", request));
        var user = await _managementUserRepository.FindByIdAsync(request.Request.Id);
        if (user == null)
        {
            _logger.Here().Information("User not found");
            activity.SetTag(TrackerConstants.CommandStatus, "NotFound");
            return Result<ManagementUserResponse>.Failure(ErrorCodes.NotFound);
        }
        
        user.UpdateActiveStatus(request.Request.IsActive);
        var userRoles = user.GetUserRoleMappings();

        
        // remove all roles from user
        foreach (var role in userRoles)
        {
            await _managementUserRepository.RemoveFromRoleAsync(user, role);
        }
        
        if (!await _managementUserRepository.AddToRoleAsync(user, request.Request.Role))
        {
            _logger.Here().Error("Failed to add role to user");
            activity.SetTag(TrackerConstants.CommandStatus, "RoleAssignmentFailed");
            return Result<ManagementUserResponse>.Failure(ErrorCodes.OperationFailed);
        }

        UpdateProfileMetadata(user, request.RequestInformation.CurrentUser.Id);

        if (!await _managementUserRepository.UpdateUser(user))
        {
            _logger.Here().Error("Failed to update user");
            activity.SetTag(TrackerConstants.CommandStatus, "UserUpdateFailed");
            return Result<ManagementUserResponse>.Failure(ErrorCodes.OperationFailed);
        }
        
        // TODO: Publish events
        await PublishEvents(user, request.RequestInformation);

        activity.SetTag(TrackerConstants.CommandStatus, "Success");
        _logger.Here().Information("Management user role updated successfully");
        _logger.Here().MethodExited();

        return Result<ManagementUserResponse>.Success(_mapper.Map<ManagementUserResponse>(user));
    }

    private static void UpdateProfileMetadata(Domain.Entities.Management.ManagementUser managementUser, string userId)
    {
        managementUser.SetCreatedBy(userId);
        managementUser.SetUpdatedBy(userId);
        managementUser.SetUpdationTime();
    }

    private async Task PublishEvents(Domain.Entities.Management.ManagementUser managementUser, RequestInformation requestInformation)
    {
        // Publish management user role updated event
        var managementUserRoleUpdatedPublishService = _publishServiceFactory.CreatePublishService<Domain.Entities.Management.ManagementUser, ManagementUserUpdated>();
        await managementUserRoleUpdatedPublishService.PublishAsync(managementUser, requestInformation.CorrelationId);
    }
}
