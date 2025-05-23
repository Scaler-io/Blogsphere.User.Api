using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Blogsphere.User.Infrastructure.Security;
public class ConfirmationEmailTokenProvider<TUser>(IDataProtectionProvider dataProtectionProvider,
    IOptions<DataProtectionTokenProviderOptions> options,
    ILogger<DataProtectorTokenProvider<TUser>> logger) : DataProtectorTokenProvider<TUser>(dataProtectionProvider, options, logger) where TUser : IdentityUser
{


}
