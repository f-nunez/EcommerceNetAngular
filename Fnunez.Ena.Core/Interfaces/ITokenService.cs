using Fnunez.Ena.Core.Entities.Identity;

namespace Fnunez.Ena.Core.Interfaces;

public interface ITokenService
{
    string CreateToken(AppUser appUser);
}