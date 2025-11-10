using AutoMapper;
using Blogsphere.User.Application.Contracts.ActivityTracker;
using Blogsphere.User.Application.Contracts.CQRS;
using Blogsphere.User.Application.Contracts.Data.Repositories;
using Blogsphere.User.Application.Extensions;
using Blogsphere.User.Domain.Models.Constants;
using Blogsphere.User.Domain.Models.Core;
using Blogsphere.User.Domain.Models.Enums;
using Blogsphere.User.Domain.Models.Responses;

namespace Blogsphere.User.Application.Features.ManagementUser.Queries.GetManagementUserById;

public class GetManagementUserByIdQueryHandler(IManagementUserRepository managementUserRepository, ILogger logger, IActivityTracker activityTracker, IMapper mapper) : IQueryHandler<GetManagementUserByIdQuery, Result<ManagementUserResponse>>
{
    private readonly IManagementUserRepository _managementUserRepository = managementUserRepository;
    private readonly ILogger _logger = logger;
    private readonly IActivityTracker _activityTracker = activityTracker;
    private readonly IMapper _mapper = mapper;

    public async Task<Result<ManagementUserResponse>> Handle(GetManagementUserByIdQuery request, CancellationToken cancellationToken)
    {
        _logger.Here().MethodEntered();
        _logger.Here().Information("Query executing {name}", nameof(GetManagementUserByIdQuery));

        using var activity = _activityTracker.TrackCommandActivity(nameof(GetManagementUserByIdQuery), ("request", request));

        var user = await _managementUserRepository.FindByIdAsync(request.Id);
        if (user == null)
        {
            _logger.Here().Information("User not found");
            activity.SetTag(TrackerConstants.CommandStatus, "NotFound");
            return Result<ManagementUserResponse>.Failure(ErrorCodes.NotFound);
        }

        var ManagementUserResponse = _mapper.Map<ManagementUserResponse>(user);
        activity.SetTag(TrackerConstants.CommandStatus, "Success");
        _logger.Here().Information("User found {name}", user.FullName);
        _logger.Here().MethodExited();
        return Result<ManagementUserResponse>.Success(ManagementUserResponse);
    }
}
