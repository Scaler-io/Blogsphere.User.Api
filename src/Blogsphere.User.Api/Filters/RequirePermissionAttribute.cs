using Blogsphere.User.Api.Services;
using Blogsphere.User.Application.Contracts.Security;
using Blogsphere.User.Application.Extensions;
using Blogsphere.User.Domain.Models.Core;
using Blogsphere.User.Domain.Models.Enums;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Blogsphere.User.Api.Filters;

public class RequirePermissionAttribute : TypeFilterAttribute
{
    public RequirePermissionAttribute(ApiAccess requiredPermission) : base(typeof(RequirePermissionExecutor))
    {
        Arguments = [requiredPermission];
    }

    [AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
    public class RequirePermissionExecutor(IIdentityService identityService, 
        ILogger logger, 
        IPermissionMapper mapper, 
        ApiAccess requiredRole) : Attribute, IActionFilter
    {
        private readonly IIdentityService _identityService = identityService;
        private readonly ILogger _logger = logger;
        private readonly IPermissionMapper _permissionMapper = mapper;
        private readonly ApiAccess _requiredRole = requiredRole;

        public void OnActionExecuted(ActionExecutedContext context)
        {

        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            _logger.Here().MethodEntered();

            var currentUser = _identityService.PrepareUser();
            List<string> requiredPermission = [.. _permissionMapper.GetPermissionsForRole(_requiredRole)];

            var commonPermission = requiredPermission.Intersect(currentUser.Authorization.Permissions).ToList();

            if (!commonPermission.Any())
            {
                _logger.Here().Error("No matching permission found");
                context.Result = new UnauthorizedObjectResult(new ApiError(nameof(ErrorCodes.Unauthorized), "Access denied"));
            }
            _logger.Here().MethodExited();
        }
    }
}
