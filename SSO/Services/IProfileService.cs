using System.Security.Claims;
using IdentityServer4.Models;
using IdentityServer4.Services;

namespace SSO.Services;

public class  ProfileService : IProfileService
{
    public async Task GetProfileDataAsync(ProfileDataRequestContext context)
    {
        var user = context.Subject;
        var userClaims = new List<Claim>
        {
            new Claim("custom_claim_1", "value_1"),
            new Claim("custom_claim_2", "value_2")
            // Thêm các claim tùy chỉnh khác ở đây
        }; // Thêm các claim tùy chỉnh vào context.IssuedClaims
        context.IssuedClaims.AddRange(userClaims);


        await Task.CompletedTask;
    }

    public async Task IsActiveAsync(IsActiveContext context)
    {
        context.IsActive = true;
        await Task.CompletedTask;
    }
}