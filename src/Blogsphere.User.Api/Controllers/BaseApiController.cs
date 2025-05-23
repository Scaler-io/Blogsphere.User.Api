using System.Net;
using Blogsphere.User.Api.Extensions;
using Blogsphere.User.Api.Services;
using Blogsphere.User.Domain.Models.Core;
using Blogsphere.User.Domain.Models.Dtos;
using Blogsphere.User.Domain.Models.Enums;
using Microsoft.AspNetCore.Mvc;

namespace Blogsphere.User.Api.Controllers;

[ApiController]
[Route("api/v{version:apiVersion}/[controller]")]
public class BaseApiController(ILogger logger, IIdentityService identityService) : ControllerBase
{
    protected ILogger Logger { get; set; } = logger;
    protected readonly IIdentityService _identityService = identityService;

    protected UserDto CurrentUser => new(); // will change later

    protected RequestInformation RequestInformation => new() 
    {
        CorrelationId = GetOrGenerateCorrelationId(),
        CurrentUser = User.Identity.IsAuthenticated ? CurrentUser : new()       
    };

    private string GetOrGenerateCorrelationId() => Request?.GetRequestHeaderOrDefault("CorrelationId", $"GEN-{Guid.NewGuid()}");

    protected IActionResult OkOrFailure<T>(Result<T> result)
    {
        if (result == null)
        {
            return NotFound(new ApiResponse(ErrorCodes.NotFound));
        }

        if (result.IsSuccess && result.Data == null)
        {
            return NotFound(new ApiResponse(ErrorCodes.NotFound));
        }

        if (result.IsSuccess && result.Data != null)
        {
            return Ok(result.Data);
        }

        return result.ErrorCode switch
        {
            ErrorCodes.BadRequest => BadRequest(new ApiValidationResponse(result.ErrorMessage)),
            ErrorCodes.InternalServerError => InternalServerError(new ApiExceptionResponse(result.ErrorMessage)),
            ErrorCodes.NotFound => NotFound(new ApiResponse(ErrorCodes.NotFound, result.ErrorMessage)),
            ErrorCodes.Unauthorized => Unauthorized(new ApiResponse(ErrorCodes.Unauthorized, result.ErrorMessage)),
            ErrorCodes.OperationFailed => BadRequest(new ApiResponse(ErrorCodes.OperationFailed, result.ErrorMessage)),
            ErrorCodes.NotAllowed => BadRequest(new ApiResponse(ErrorCodes.NotAllowed, result.ErrorMessage)),
            _ => BadRequest(new ApiResponse(ErrorCodes.BadRequest, result.ErrorMessage))
        };
    }

    private static ObjectResult InternalServerError(ApiResponse response)
    {
        return new ObjectResult(response)
        {
            StatusCode = (int)HttpStatusCode.InternalServerError,
            ContentTypes =
            [
                "application/json"
            ]
        };
    }
}
