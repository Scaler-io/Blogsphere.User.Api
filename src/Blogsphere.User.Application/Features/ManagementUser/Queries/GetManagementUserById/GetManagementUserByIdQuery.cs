using Blogsphere.User.Application.Contracts.CQRS;
using Blogsphere.User.Domain.Models.Core;
using Blogsphere.User.Domain.Models.Responses;

namespace Blogsphere.User.Application.Features.ManagementUser.Queries.GetManagementUserById;

public class GetManagementUserByIdQuery(string id) : IQuery<Result<ManagementUserResponse>>
{
    public string Id { get; set; } = id;
}
