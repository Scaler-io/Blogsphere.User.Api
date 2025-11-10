using AutoMapper;
using Blogsphere.User.Application.Contracts.ActivityTracker;
using Blogsphere.User.Application.Contracts.CQRS;
using Blogsphere.User.Application.Contracts.Data.Repositories;
using Blogsphere.User.Application.Extensions;
using Blogsphere.User.Domain.Entities;
using Blogsphere.User.Domain.Models.Constants;
using Blogsphere.User.Domain.Models.Core;
using Blogsphere.User.Domain.Models.Responses;
using Microsoft.EntityFrameworkCore;

namespace Blogsphere.User.Application.Features.AppUser.Queries.GetAllAppUsers;

public class GetAllAppUsersQueryHandler(ILogger logger, IUserRepository userRepository, IMapper mapper, IActivityTracker activityTracker) : IQueryHandler<GetAllAppUsersQuery, Result<PaginatedResult<AppUserResponse>>>
{
    private readonly ILogger _logger = logger;
    private readonly IUserRepository _userRepository = userRepository;
    private readonly IMapper _mapper = mapper;
    private readonly IActivityTracker _activityTracker = activityTracker;

    public async Task<Result<PaginatedResult<AppUserResponse>>> Handle(GetAllAppUsersQuery request, CancellationToken cancellationToken)
    {
        _logger.Here().MethodEntered();
        _logger.Here().Information("Query executing {name}", nameof(GetAllAppUsersQuery));

        using var activity = _activityTracker.TrackCommandActivity(nameof(GetAllAppUsersQuery), ("request", request));
        var (users, totalCount) = await GetAllAppUsersAsync(request.PaginationRequest);
        if(users.Count == 0 || users == null)
        {
            _logger.Here().Information("No app users found");
            activity.SetTag(TrackerConstants.CommandStatus, "NotFound");
            return Result<PaginatedResult<AppUserResponse>>.Success(new PaginatedResult<AppUserResponse>()
            {
                Items = [],
                TotalCount = 0,
                PageNumber = request.PaginationRequest.PageNumber,
                PageSize = request.PaginationRequest.PageSize,
            });
        }

        var AppUserResponse = _mapper.Map<List<AppUserResponse>>(users);
        var paginatedResult = new PaginatedResult<AppUserResponse>()
        {
            Items = AppUserResponse,
            TotalCount = totalCount,
            PageNumber = request.PaginationRequest.PageNumber,
            PageSize = request.PaginationRequest.PageSize,
        };

        activity.SetTag(TrackerConstants.CommandStatus, "Success");
        _logger.Here().Information("{0} management users found", totalCount);
        _logger.Here().MethodExited();
        return Result<PaginatedResult<AppUserResponse>>.Success(paginatedResult);
    }

    private async Task<(List<ApplicationUser> users, int totalCount)> GetAllAppUsersAsync(PaginationRequest paginationRequest)
    {
        var query = _userRepository.AsQueryable();
        var totalCount = await query.CountAsync();
        var items = await query
        .Skip((paginationRequest.PageNumber - 1) * paginationRequest.PageSize)
        .Take(paginationRequest.PageSize)
        .ToListAsync();
        return (items, totalCount);
    }
}
