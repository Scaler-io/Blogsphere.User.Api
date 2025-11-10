using Serilog;

namespace Blogsphere.User.Infrastructure.Security.ManagementUser;

public class ManagementTwoFactorAuthTokenProvider(ILogger logger) : BaseTwoFactorTokenProvider<Domain.Entities.Management.ManagementUser>(logger, "ManagementUser")
{
    
}
