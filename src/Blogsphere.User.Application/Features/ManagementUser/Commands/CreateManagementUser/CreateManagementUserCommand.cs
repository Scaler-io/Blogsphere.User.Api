using Blogsphere.User.Application.Contracts.CQRS;
using Blogsphere.User.Domain.Attributes;
using Blogsphere.User.Domain.Models.Core;
using Blogsphere.User.Domain.Models.Requests.ManagementUser;
using Blogsphere.User.Domain.Models.Responses;

namespace Blogsphere.User.Application.Features.ManagementUser.Commands.CreateManagementUser;

public class CreateManagementUserCommand(
    CreateManagementUserRequest createManagementUserRequest,
    RequestInformation requestInformation) : ICommand<Result<ManagementUserResponse>>
{
    [ValidateNested]
    public CreateManagementUserRequest CreateManagementUserRequest { get; set; } = createManagementUserRequest;
    public RequestInformation RequestInformation { get; set; } = requestInformation;
}
