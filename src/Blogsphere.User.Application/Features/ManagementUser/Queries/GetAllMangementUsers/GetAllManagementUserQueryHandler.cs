using AutoMapper;
using Blogsphere.User.Application.Contracts.ActivityTracker;
using Blogsphere.User.Application.Contracts.CQRS;
using Blogsphere.User.Application.Contracts.Data.Repositories;
using Blogsphere.User.Application.Extensions;
using Blogsphere.User.Domain.Models.Constants;
using Blogsphere.User.Domain.Models.Core;
using Blogsphere.User.Domain.Models.Responses;
using Microsoft.EntityFrameworkCore;

namespace Blogsphere.User.Application.Features.ManagementUser.Queries.GetAllMangementUsers;

public class GetAllManagementUserQueryHandler(ILogger logger, IManagementUserRepository managementUserRepository, IMapper mapper, IActivityTracker activityTracker) : IQueryHandler<GetAllManagementUserQuery, 
Result<PaginatedResult<ManagementUserResponse>>>
{
    private readonly ILogger _logger = logger;
    private readonly IManagementUserRepository _managementUserRepository = managementUserRepository;
    private readonly IMapper _mapper = mapper;
    private readonly IActivityTracker _activityTracker = activityTracker;

    public async Task<Result<PaginatedResult<ManagementUserResponse>>> Handle(GetAllManagementUserQuery request, CancellationToken cancellationToken)
    {
        _logger.Here().MethodEntered();
        _logger.Here().Information("Query executing {name}", nameof(GetAllManagementUserQuery));

        using var activity = _activityTracker.TrackCommandActivity(nameof(GetAllManagementUserQuery), ("request", request));

        var (users, totalCount) = await GetManagementUsersAsync(request.PaginationRequest);
        if(users.Count == 0 || users == null){
            _logger.Here().Information("No management users found");
            activity.SetTag(TrackerConstants.CommandStatus, "NotFound");
            return Result<PaginatedResult<ManagementUserResponse>>.Success(new PaginatedResult<ManagementUserResponse>()
            {
                Items = [],
                TotalCount = 0,
                PageNumber = request.PaginationRequest.PageNumber,
                PageSize = request.PaginationRequest.PageSize,
            });
        }

        var ManagementUserResponse = _mapper.Map<List<ManagementUserResponse>>(users);
        var paginatedResult = new PaginatedResult<ManagementUserResponse>()
        {
            Items = ManagementUserResponse,
            TotalCount = totalCount,
            PageNumber = request.PaginationRequest.PageNumber,
            PageSize = request.PaginationRequest.PageSize,
        };

        activity.SetTag(TrackerConstants.CommandStatus, "Success");
        _logger.Here().Information("{0} management users found", totalCount);
        _logger.Here().MethodExited();
        return Result<PaginatedResult<ManagementUserResponse>>.Success(paginatedResult);
    }

    private async Task<(List<Domain.Entities.Management.ManagementUser> users, int totalCount)> GetManagementUsersAsync(PaginationRequest paginationRequest){
        var query = _managementUserRepository.AsQueryable();
        var totalCount = await query.CountAsync();
        var items = await query
        
            .Skip((paginationRequest.PageNumber - 1) * paginationRequest.PageSize)
            .Take(paginationRequest.PageSize)
            .ToListAsync();
        return (items, totalCount);
    }
}
