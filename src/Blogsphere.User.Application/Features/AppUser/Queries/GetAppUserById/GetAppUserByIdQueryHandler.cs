using AutoMapper;
using Blogsphere.User.Application.Contracts.ActivityTracker;
using Blogsphere.User.Application.Contracts.CQRS;
using Blogsphere.User.Application.Contracts.Data.Repositories;
using Blogsphere.User.Application.Extensions;
using Blogsphere.User.Domain.Models.Constants;
using Blogsphere.User.Domain.Models.Core;
using Blogsphere.User.Domain.Models.Enums;
using Blogsphere.User.Domain.Models.Responses;
using Microsoft.EntityFrameworkCore;

namespace Blogsphere.User.Application.Features.AppUser.Queries.GetAppUserById;

public class GetAppUserByIdQueryHandler(
    IUserRepository userRepository,
    ILogger logger,
    IActivityTracker activityTracker,
    IMapper mapper) : IQueryHandler<GetAppUserByIdQuery, Result<AppUserResponse>>
{

    private readonly IUserRepository _userRepository = userRepository;
    private readonly ILogger _logger = logger;
    private readonly IActivityTracker _activityTracker = activityTracker;
    private readonly IMapper _mapper = mapper;
    
    public async Task<Result<AppUserResponse>> Handle(GetAppUserByIdQuery request, CancellationToken cancellationToken)
    {
        _logger.Here().MethodEntered();
        _logger.Here().Information("Query executing {name}", nameof(GetAppUserByIdQuery));

        using var activity = _activityTracker.TrackCommandActivity(nameof(GetAppUserByIdQuery), ("request", request));

        var user = await _userRepository.AsQueryable().FirstOrDefaultAsync(x => x.Id == request.Id);
        if (user == null)
        {
            _logger.Here().Information("User not found");
            activity.SetTag(TrackerConstants.CommandStatus, "NotFound");
            return Result<AppUserResponse>.Failure(ErrorCodes.NotFound);
        }

        var appUserResponse = _mapper.Map<AppUserResponse>(user);
        activity.SetTag(TrackerConstants.CommandStatus, "Success");
        _logger.Here().Information("User found {name}", user.UserName);
        _logger.Here().MethodExited();
        return Result<AppUserResponse>.Success(appUserResponse);
    }
}
