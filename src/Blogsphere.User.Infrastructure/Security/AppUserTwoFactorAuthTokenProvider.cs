using Blogsphere.User.Domain.Entities;
using Serilog;

namespace Blogsphere.User.Infrastructure.Security;

public class AppUserTwoFactorAuthTokenProvider(ILogger logger) : BaseTwoFactorTokenProvider<ApplicationUser>(logger, "ApplicationUser")
{
}
