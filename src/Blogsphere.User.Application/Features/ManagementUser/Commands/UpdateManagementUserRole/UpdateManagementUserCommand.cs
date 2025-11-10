using Blogsphere.User.Application.Contracts.CQRS;
using Blogsphere.User.Domain.Attributes;
using Blogsphere.User.Domain.Models.Core;
using Blogsphere.User.Domain.Models.Requests.ManagementUser;
using Blogsphere.User.Domain.Models.Responses;

namespace Blogsphere.User.Application.Features.ManagementUser.Commands.UpdateManagementUserRole;

public class UpdateManagementUserRoleCommand(
    UpdateManagementUserRoleRequest request,
    RequestInformation requestInformation) 
    : ICommand<Result<ManagementUserResponse>>
{
    [ValidateNested]
    public UpdateManagementUserRoleRequest Request { get; set; } = request;
    public RequestInformation RequestInformation { get; set; } = requestInformation;
}
