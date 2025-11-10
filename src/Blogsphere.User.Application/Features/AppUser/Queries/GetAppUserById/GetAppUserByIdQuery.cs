using Blogsphere.User.Application.Contracts.CQRS;
using Blogsphere.User.Domain.Models.Core;
using Blogsphere.User.Domain.Models.Responses;

namespace Blogsphere.User.Application.Features.AppUser.Queries.GetAppUserById;

public class GetAppUserByIdQuery(string id) : IQuery<Result<AppUserResponse>>
{
    public string Id { get; set; } = id;
}
