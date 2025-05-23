using Blogsphere.User.Domain.Models.Dtos;
using Newtonsoft.Json;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace Blogsphere.User.Api.Services;

public class IdentityService(IHttpContextAccessor httpContextAccessor) : IIdentityService
{

    private readonly IHttpContextAccessor _httpContextAccessor = httpContextAccessor;

    public const string IdClaim = ClaimTypes.NameIdentifier;
    public const string RoleClaim = ClaimTypes.Role;
    public const string FirstNameClaim = ClaimTypes.GivenName;
    public const string LastNameClaim = ClaimTypes.Surname;
    public const string UsernameClaim = JwtRegisteredClaimNames.Name;
    public const string EmailClaim = ClaimTypes.Email;
    public const string PermissionClaim = "permissions";

    public UserDto PrepareUser()
    {
        var claims = _httpContextAccessor.HttpContext.User.Claims;
        var token = _httpContextAccessor.HttpContext.Request.Headers.Authorization;

        var roleString = claims.FirstOrDefault(c => c.Type == RoleClaim).Value;
        var permissionString = claims.FirstOrDefault(c => c.Type == PermissionClaim).Value;

        return new()
        {
            Id = claims.FirstOrDefault(c => c.Type == IdClaim).Value,
            FirstName = claims.FirstOrDefault(c => c.Type == FirstNameClaim).Value,
            LastName = claims.FirstOrDefault(c => c.Type == LastNameClaim).Value,
            UserName = claims.FirstOrDefault(c => c.Type == UsernameClaim).Value,
            Email = claims.FirstOrDefault(c => c.Type == EmailClaim).Value,
            Authorization = new()
            {
                Roles = JsonConvert.DeserializeObject<List<string>>(roleString),
                Permissions = JsonConvert.DeserializeObject<List<string>>(permissionString),
                Token = token
            }
        };

    }
}
