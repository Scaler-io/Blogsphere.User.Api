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
    public RequirePermissionAttribute(ApiAccess permission, params string[] scopes) : base(typeof(RequirePermissionExecutor))
    {
        Arguments = [permission, scopes];
    }

    [AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
    public class RequirePermissionExecutor(IIdentityService identityService, 
        ILogger logger, 
        IPermissionMapper mapper, 
        ApiAccess requiredRole,
        params string[] scopes) : Attribute, IActionFilter
    {
        private readonly IIdentityService _identityService = identityService;
        private readonly ILogger _logger = logger;
        private readonly IPermissionMapper _permissionMapper = mapper;
        private readonly ApiAccess _requiredRole = requiredRole;
        private readonly List<string> _scopes = [.. scopes];

        public void OnActionExecuted(ActionExecutedContext context)
        {

        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            _logger.Here().MethodEntered();
            _logger.Here().Debug("Required scopes: {Scopes}", string.Join(", ", _scopes));

            
            if(_scopes.Any()){
                var availableScopes = context.HttpContext
                    .User
                    .Claims
                    .Where(c => c.Type == "scope")
                    .Select(c => c.Value)
                    .ToList();

                    _logger.Here().Debug("Available scopes: {AvailableScopes}", string.Join(", ", availableScopes));

                if(_scopes.Any(s => !availableScopes.Contains(s)))
                {
                    _logger.Here().Error("No matching scope found");
                    context.Result = new UnauthorizedObjectResult(new ApiResponse(ErrorCodes.Unauthorized, "Access denied"));
                    return;
                }
            }

            var isM2MRequest = context.HttpContext.Request.Headers.TryGetValue("X-M2M-Request", out var m2mRequest);
            if (isM2MRequest)
            {
                _logger.Here().Debug("M2M request detected, skipping permission check");
                return;
            }

            var currentUser = _identityService.PrepareUser();
            if(currentUser.Authorization.Permissions.Contains("*"))
            {
                _logger.Here().Debug("All permissions are allowed, skipping permission check");
                return;
            }

            // Alternative optimization for larger permission sets:
            // Convert to HashSet for O(1) lookups instead of O(n) Contains operations
            var requiredPermissions = _permissionMapper.GetPermissionsForRole(_requiredRole);
            var requiredPermissionsSet = requiredPermissions.ToHashSet();

            // Short-circuit evaluation: stops as soon as first match is found
            var hasRequiredPermission = currentUser.Authorization.Permissions
                .Any(requiredPermissionsSet.Contains);

            if (!hasRequiredPermission)
            {
                _logger.Here().Error("No matching permission found");
                context.Result = new UnauthorizedObjectResult(new ApiResponse(ErrorCodes.Unauthorized, "Access denied"));
            }

            _logger.Here().MethodExited();
        }
    }
}
