using System.Security.Claims;
using Fnunez.Ena.Core.Entities.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Fnunez.Ena.API.Extensions;

public static class UserManagerExtension
{
    public static async Task<AppUser> FindUserByEmailFromClaimsPrincipalAsync(
        this UserManager<AppUser> userManager, ClaimsPrincipal userClaims)
    {
        string email = userClaims?.Claims?
            .FirstOrDefault(x => x.Type == ClaimTypes.Email)?.Value;

        return await userManager.Users
            .SingleOrDefaultAsync(x => x.Email == email);
    }

    public static async Task<AppUser> FindUserByEmailFromClaimsPrincipalWithAddressAsync(
        this UserManager<AppUser> userManager, ClaimsPrincipal userClaims)
    {
        string email = userClaims?.Claims?
            .FirstOrDefault(x => x.Type == ClaimTypes.Email)?.Value;

        return await userManager.Users.Include(x => x.Address)
            .SingleOrDefaultAsync(x => x.Email == email);
    }
}