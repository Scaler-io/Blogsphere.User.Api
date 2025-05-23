using Blogsphere.User.Domain.Models.Dtos;

namespace Blogsphere.User.Domain.Models.Core;

public class RequestInformation
{
    public UserDto CurrentUser { get; set; }
    public string CorrelationId { get; set; }
}
