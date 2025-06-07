using Asp.Versioning;
using Blogsphere.Swagger;
using Blogsphere.Swagger.Examples;
using Blogsphere.Swagger.Examples.UserRegistration;
using Blogsphere.User.Api.Services;
using Blogsphere.User.Application.Features.UserRegistration.Commands;
using Blogsphere.User.Domain.Models.Core;
using Blogsphere.User.Domain.Models.Requests.Registration;
using Blogsphere.User.Domain.Models.Responses;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using Swashbuckle.AspNetCore.Filters;
using Blogsphere.User.Application.Extensions;

namespace Blogsphere.User.Api.Controllers.v2;

[ApiVersion("2")]
public class RegistrationController(ILogger logger, IMediator mediator, IIdentityService identityService) 
    : BaseApiController(logger, identityService)
{
    private readonly IMediator _mediator = mediator;

    [HttpPost]
    [SwaggerHeader("CorrelationId", Description = "expects unique correlation id")]
    [SwaggerOperation(OperationId = "RegisterUser", Description = "Fetches all user list")]
    [SwaggerRequestExample(typeof(RegistrationRequest), typeof(UserRegistrationRequestExample))]
    // 200
    [ProducesResponseType(typeof(UserResponse), StatusCodes.Status201Created)]
    [SwaggerResponseExample(StatusCodes.Status201Created, typeof(UserResponseExample))]
    // 404
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
    [SwaggerResponseExample(StatusCodes.Status404NotFound, typeof(NotFoundResponseExample))]
    // 400
    [ProducesResponseType(typeof(ApiValidationResponse), StatusCodes.Status400BadRequest)]
    [SwaggerResponseExample(StatusCodes.Status400BadRequest, typeof(ValidationResponseExample))]
    // // 500
    [ProducesResponseType(typeof(ApiExceptionResponse), StatusCodes.Status500InternalServerError)]
    [SwaggerResponseExample(StatusCodes.Status500InternalServerError, typeof(InternalServerErrorResponseExample))]
    public async Task<IActionResult> RegisterUser([FromBody] RegistrationRequest request)
    {
        Logger.Here().MethodEntered();
        var command = new RegisterUserCommand(request, RequestInformation);
        var result = await _mediator.Send(command);
        Logger.Here().MethodExited();
        return OkOrFailure(result);
    }
}
