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
            new Claim("Id", "cc5aaa65-fe69-4d4d-88ef-08dc6075cb4c"),
            new Claim("Roles", "DirectorGroup"),
            new Claim("Roles", "DirectorUnit"),
            new Claim("Roles", "SuperAdmin"),
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