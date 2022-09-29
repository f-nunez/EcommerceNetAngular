using Fnunez.Ena.Core.Entities.Identity;
using Fnunez.Ena.Infrastructure;
using Microsoft.AspNetCore.Identity;

namespace Fnunez.Ena.API.Extensions;

public static class IdentityServiceExtension
{
    public static IServiceCollection AddIdentityServices(this IServiceCollection services)
    {
        IdentityBuilder builder = services.AddIdentityCore<AppUser>();
        builder = new IdentityBuilder(builder.UserType, builder.Services);
        builder.AddEntityFrameworkStores<AppIdentityDbContext>();
        builder.AddSignInManager<SignInManager<AppUser>>();
        services.AddAuthentication();

        return services;
    }
}