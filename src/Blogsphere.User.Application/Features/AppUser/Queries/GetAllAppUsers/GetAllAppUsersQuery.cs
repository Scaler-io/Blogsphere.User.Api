using Blogsphere.User.Application.Contracts.CQRS;
using Blogsphere.User.Domain.Models.Core;
using Blogsphere.User.Domain.Models.Responses;

namespace Blogsphere.User.Application.Features.AppUser.Queries.GetAllAppUsers;

public class GetAllAppUsersQuery(PaginationRequest paginationRequest) : IQuery<Result<PaginatedResult<AppUserResponse>>>
{
    public PaginationRequest PaginationRequest { get; set; } = paginationRequest;
}
