using Asp.Versioning;
using Blogsphere.Swagger;
using Blogsphere.User.Api.Services;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using Swashbuckle.AspNetCore.Filters;
using Blogsphere.User.Domain.Models.Responses;
using Blogsphere.User.Domain.Models.Core;
using Blogsphere.Swagger.Examples;
using Blogsphere.User.Domain.Models.Enums;
using Blogsphere.User.Domain.Models.Constants;
using Blogsphere.User.Api.Filters;
using Blogsphere.User.Application.Extensions;
using Blogsphere.User.Application.Features.AppUser.Queries.GetAllAppUsers;
using MediatR;
using Blogsphere.Swagger.Examples.AppUser;
using Blogsphere.User.Application.Features.AppUser.Queries.GetAppUserById;
using Microsoft.AspNetCore.Authorization;

namespace Blogsphere.User.Api.Controllers.v2;

[ApiVersion("2")]
[Authorize]
public class AppUserController(ILogger logger, IIdentityService identityService, IMediator mediator) : BaseApiController(logger, identityService)
{
    private readonly IMediator _mediator = mediator;

    [HttpGet]
    [SwaggerOperation(OperationId = "GetAllAppUsers", Description = "Fetches all appication users")]
    [SwaggerHeader("CorrelationId", Description = "expects unique correlation id")]
    // 200
    [SwaggerResponseExample(StatusCodes.Status200OK, typeof(AppUserListResponseExample))]
    [ProducesResponseType(typeof(PaginatedResult<AppUserResponse>), StatusCodes.Status200OK)]
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
    public async Task<IActionResult> GetAllAppUsers([FromQuery] PaginationRequest paginationRequest)
    {
        Logger.Here().MethodEntered();
        var query = new GetAllAppUsersQuery(paginationRequest);
        var result = await _mediator.Send(query);
        Logger.Here().MethodExited();
        return OkOrFailure(result);
    }

    [HttpGet("{id}")]
    [SwaggerOperation(OperationId = "GetAppUserById", Description = "Fetches a app user by id")]
    [SwaggerHeader("CorrelationId", Description = "expects unique correlation id")]
    // 200
    [SwaggerResponseExample(StatusCodes.Status200OK, typeof(AppUserResponseExample))]
    [ProducesResponseType(typeof(AppUserResponse), StatusCodes.Status200OK)]
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
    public async Task<IActionResult> GetAppUserById([FromRoute] string id)
    {
        Logger.Here().MethodEntered();
        var query = new GetAppUserByIdQuery(id);
        var result = await _mediator.Send(query);
        Logger.Here().MethodExited();
        return OkOrFailure(result);
    }
}
