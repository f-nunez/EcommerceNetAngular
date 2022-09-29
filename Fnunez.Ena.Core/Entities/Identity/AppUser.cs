using Microsoft.AspNetCore.Identity;

namespace Fnunez.Ena.Core.Entities.Identity;

public class AppUser : IdentityUser
{
    public string DisplayName { get; set; }
    public Address Address { get; set; }
}