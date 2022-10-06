using System.Security.Claims;

namespace Fnunez.Ena.API.Extensions;

public static class ClaimsPrincipalExtension
{
    public static string RetrieveEmailFromClaimsPrincipal(this ClaimsPrincipal claimsPrincipal)
    {
        return claimsPrincipal?.Claims?.FirstOrDefault( x => x.Type == ClaimTypes.Email)?.Value;
    }
} 