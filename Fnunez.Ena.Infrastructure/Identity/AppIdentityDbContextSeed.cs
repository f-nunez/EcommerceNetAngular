using Fnunez.Ena.Core.Entities.Identity;
using Microsoft.AspNetCore.Identity;

namespace Fnunez.Ena.Infrastructure.Identity;

public class AppIdentityDbContextSeed
{
    public static async Task SeedUserAsync(UserManager<AppUser> userManager)
    {
        if (userManager.Users.Any())
            return;

        AppUser demoUser = CreateDemoUser();
        await userManager.CreateAsync(demoUser, "password");
    }

    private static AppUser CreateDemoUser()
    {
        return new AppUser
        {
            DisplayName = "Franky",
            Email = "francisco@nunez.ninja.com",
            UserName = "guestUser",
            Address = new Address
            {
                FirstName = "Francisco",
                LastName = "Nuñez",
                City = "Tijuana",
                State = "Baja California",
                Street = "Flores Magon 8665",
                ZipCode = "22000"
            }
        };
    }
}