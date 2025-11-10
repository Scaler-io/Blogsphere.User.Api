

using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;

namespace Blogsphere.User.Infrastructure.Security.ManagementUser;

public class ManagementEmailConfirmationTokenProviderOptions : DataProtectionTokenProviderOptions
{
}

public class ManagementEmailConfirmationTokenProvider<TUser>(
    IDataProtectionProvider dataProtectionProvider,
    IOptions<ManagementEmailConfirmationTokenProviderOptions> options,
    Microsoft.Extensions.Logging.ILogger<DataProtectorTokenProvider<TUser>> msLogger) : DataProtectorTokenProvider<TUser>(dataProtectionProvider, options, msLogger)
    where TUser : Domain.Entities.Management.ManagementUser
{
    public override async Task<string> GenerateAsync(string purpose, UserManager<TUser> manager, TUser user)
    {
        return await base.GenerateAsync(purpose, manager, user);
    }
}
