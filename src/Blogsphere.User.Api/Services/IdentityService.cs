using Blogsphere.User.Domain.Models.Dtos;
using Microsoft.IdentityModel.Tokens;
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

        if (claims.IsNullOrEmpty())
        {
            return null;
        }
        
       var permissionsString = claims.Where(c => c.Type == PermissionClaim).FirstOrDefault()?.Value;
        var id = claims.Where(c => c.Type == IdClaim).FirstOrDefault().Value;
        var firstName = claims.Where(c => c.Type == FirstNameClaim).FirstOrDefault().Value;
        var lastName = claims.Where(c => c.Type == LastNameClaim).FirstOrDefault().Value;
        var name = claims.Where(c => c.Type == UsernameClaim).FirstOrDefault().Value;
        var email = claims.Where(c => c.Type == EmailClaim).FirstOrDefault().Value;
        var role = claims.Where(c => c.Type == RoleClaim).FirstOrDefault()?.Value;
        var permissions = permissionsString == "*" ? ["*"] : JsonConvert.DeserializeObject<List<string>>(permissionsString);

        return new()
        {
            Id = claims.FirstOrDefault(c => c.Type == IdClaim).Value,
            FirstName = claims.FirstOrDefault(c => c.Type == FirstNameClaim).Value,
            LastName = claims.FirstOrDefault(c => c.Type == LastNameClaim).Value,
            UserName = claims.FirstOrDefault(c => c.Type == UsernameClaim).Value,
            Email = claims.FirstOrDefault(c => c.Type == EmailClaim).Value,
            Authorization = new()
            {
                Roles = [role],
                Permissions = permissions,
                Token = token
            }
        };

    }
}
