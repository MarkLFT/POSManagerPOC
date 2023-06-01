using IdentityServer4.Extensions;
using IdentityServer4.Models;
using IdentityServer4.Services;

namespace POSManager.Identity;

public class IdentityProfileService : IProfileService
{

    public Task GetProfileDataAsync(ProfileDataRequestContext context)
    {
        //var requestedClaimTypes = context.RequestedClaimTypes;
        var user = context.Subject;

        context.IssuedClaims.AddRange(user.Claims);

        return Task.CompletedTask;
    }

    public Task IsActiveAsync(IsActiveContext context)
    {
        context.IsActive = context.Subject.IsAuthenticated();
        return Task.CompletedTask;
    }
}
