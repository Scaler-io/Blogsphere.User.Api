using Blogsphere.User.Application.Contracts.CQRS;
using Blogsphere.User.Domain.Models.Core;
using Blogsphere.User.Domain.Models.Responses;

namespace Blogsphere.User.Application.Features.ManagementUser.Queries.GetAllMangementUsers;

public class GetAllManagementUserQuery(PaginationRequest paginationRequest) : IQuery<Result<PaginatedResult<ManagementUserResponse>>>
{
    public PaginationRequest PaginationRequest { get; set; } = paginationRequest;
}
