using Asp.Versioning;
using Blogsphere.User.Api.Services;
using Blogsphere.User.Domain.Models.Core;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Blogsphere.User.Application.Extensions;
using Blogsphere.User.Application.Features.ManagementUser.Queries.GetAllMangementUsers;
using Blogsphere.User.Api.Filters;
using Blogsphere.User.Domain.Models.Enums;
using Blogsphere.User.Domain.Models.Constants;
using Swashbuckle.AspNetCore.Annotations;
using Blogsphere.Swagger;
using Swashbuckle.AspNetCore.Filters;
using Blogsphere.Swagger.Examples.ManagementUser;
using Blogsphere.User.Domain.Models.Responses;
using Blogsphere.Swagger.Examples;
using Blogsphere.User.Application.Features.ManagementUser.Queries.GetManagementUserById;
using Blogsphere.User.Domain.Models.Requests.ManagementUser;
using Blogsphere.User.Application.Features.ManagementUser.Commands.CreateManagementUser;
using Blogsphere.User.Application.Features.ManagementUser.Commands.UpdateManagementUserRole;

namespace Blogsphere.User.Api.Controllers.v2;

[ApiVersion("2")]
[Authorize]
public class ManagementUserController(ILogger logger, IIdentityService identityService, IMediator mediator) 
: BaseApiController(logger, identityService)
{
    private readonly IMediator _mediator = mediator;

    [HttpGet]
    [SwaggerOperation(OperationId = "GetAllManagementUsers", Description = "Fetches all management users")]
    [SwaggerHeader("CorrelationId", Description = "expects unique correlation id")]
    // 200
    [SwaggerResponseExample(StatusCodes.Status200OK, typeof(ManagementUserListExample))]
    [ProducesResponseType(typeof(PaginatedResult<ManagementUserResponse>), StatusCodes.Status200OK)]
    // 404
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
    [SwaggerResponseExample(StatusCodes.Status404NotFound, typeof(NotFoundResponseExample))]
    // 401
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status401Unauthorized)]
    [SwaggerResponseExample(StatusCodes.Status401Unauthorized, typeof(UnAuthorizedResponseExample))]
    // 500
    [ProducesResponseType(typeof(ApiExceptionResponse), StatusCodes.Status500InternalServerError)]
    [SwaggerResponseExample(StatusCodes.Status500InternalServerError, typeof(InternalServerErrorResponseExample))]
    [RequirePermission(ApiAccess.UserView, ApiScopes.UserApiRead)]
    public async Task<IActionResult> GetAllManagementUsers([FromQuery] PaginationRequest paginationRequest)
    {
        Logger.Here().MethodEntered();
        var query = new GetAllManagementUserQuery(paginationRequest);
        var result = await _mediator.Send(query);
        Logger.Here().MethodExited();
        return OkOrFailure(result);
    }

    [HttpGet("{id}")]
    [SwaggerOperation(OperationId = "GetManagementUserById", Description = "Fetches a management user by id")]
    [SwaggerHeader("CorrelationId", Description = "expects unique correlation id")]
    // 200
    [SwaggerResponseExample(StatusCodes.Status200OK, typeof(ManagementUseResponseExample))]
    [ProducesResponseType(typeof(ManagementUserResponse), StatusCodes.Status200OK)]
    // 404
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
    [SwaggerResponseExample(StatusCodes.Status404NotFound, typeof(NotFoundResponseExample))]
    // 401
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status401Unauthorized)]
    [SwaggerResponseExample(StatusCodes.Status401Unauthorized, typeof(UnAuthorizedResponseExample))]
    // 500
    [ProducesResponseType(typeof(ApiExceptionResponse), StatusCodes.Status500InternalServerError)]
    [SwaggerResponseExample(StatusCodes.Status500InternalServerError, typeof(InternalServerErrorResponseExample))]
    [RequirePermission(ApiAccess.UserView, ApiScopes.UserApiRead)]
    public async Task<IActionResult> GetManagementUserById([FromRoute] string id)
    {
        Logger.Here().MethodEntered();
        var query = new GetManagementUserByIdQuery(id);
        var result = await _mediator.Send(query);
        Logger.Here().MethodExited();
        return OkOrFailure(result);
    }

    [HttpPost]
    [SwaggerOperation(OperationId = "GetManagementUserById", Description = "Fetches a management user by id")]
    [SwaggerHeader("CorrelationId", Description = "expects unique correlation id")]
    // 201
    [SwaggerResponseExample(StatusCodes.Status201Created, typeof(ManagementUseResponseExample))]
    [ProducesResponseType(typeof(ManagementUserResponse), StatusCodes.Status201Created)]
    // 401
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status401Unauthorized)]
    [SwaggerResponseExample(StatusCodes.Status401Unauthorized, typeof(UnAuthorizedResponseExample))]
    // 500
    [ProducesResponseType(typeof(ApiExceptionResponse), StatusCodes.Status500InternalServerError)]
    [SwaggerResponseExample(StatusCodes.Status500InternalServerError, typeof(InternalServerErrorResponseExample))]
    [RequirePermission(ApiAccess.UserCreate, ApiScopes.UserApiWrite)]
    public async Task<IActionResult> CreateManagementUser([FromBody] CreateManagementUserRequest request)
    {
        Logger.Here().MethodEntered();
        var command = new CreateManagementUserCommand(request, RequestInformation);
        var result = await _mediator.Send(command);
        Logger.Here().MethodExited();
        return OkOrFailure(result);
    }

    [HttpPut("update")]
    [SwaggerOperation(OperationId = "UpdateManagementUserRole", Description = "Updates a management user role")]
    [SwaggerHeader("CorrelationId", Description = "expects unique correlation id")]
    [SwaggerRequestExample(typeof(UpdateManagementUserRoleRequest), typeof(UpdateManagementUserRoleRequestExample))]
    // 200
    [SwaggerResponseExample(StatusCodes.Status200OK, typeof(ManagementUseResponseExample))]
    [ProducesResponseType(typeof(ManagementUserResponse), StatusCodes.Status200OK)]
    // 401
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status401Unauthorized)]
    [SwaggerResponseExample(StatusCodes.Status401Unauthorized, typeof(UnAuthorizedResponseExample))]
    // 500
    [ProducesResponseType(typeof(ApiExceptionResponse), StatusCodes.Status500InternalServerError)]
    [SwaggerResponseExample(StatusCodes.Status500InternalServerError, typeof(InternalServerErrorResponseExample))]
    [RequirePermission(ApiAccess.UserUpdate, ApiScopes.UserApiWrite)]
    public async Task<IActionResult> UpdateManagementUserRole([FromBody] UpdateManagementUserRoleRequest request)
    {
        Logger.Here().MethodEntered();
        var command = new UpdateManagementUserRoleCommand(request, RequestInformation);
        var result = await _mediator.Send(command);
        Logger.Here().MethodExited();
        return OkOrFailure(result);
    }
}
