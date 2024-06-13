using IdentityServer4.Models;
using IdentityServer4.Services;

namespace SSO.Services;

public class  ProfileService : IProfileService
{
    public async Task GetProfileDataAsync(ProfileDataRequestContext context)
    {
        var user = context.Subject;

        await Task.CompletedTask;
    }

    public async Task IsActiveAsync(IsActiveContext context)
    {
        await Task.CompletedTask;
    }
}