using Asp.Versioning;
using Blogsphere.User.Api.Services;
using Microsoft.AspNetCore.Mvc;

namespace Blogsphere.User.Api.Controllers.v1;

[ApiVersion("1")]
public class HealthCheckController(ILogger logger, IIdentityService identityService) 
    : BaseApiController(logger, identityService)
{

    [HttpGet]
    public IActionResult GetApiHealthState() => Ok("Api health is ok");
}
