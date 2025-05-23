using Blogsphere.User.Domain.Models.Dtos;

namespace Blogsphere.User.Api.Services;

public interface IIdentityService
{
    UserDto PrepareUser();
}
