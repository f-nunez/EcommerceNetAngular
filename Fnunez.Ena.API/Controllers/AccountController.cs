using System.Net;
using Fnunez.Ena.API.Dtos;
using Fnunez.Ena.API.Errors;
using Fnunez.Ena.Core.Entities.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Fnunez.Ena.API.Controllers;

public class AccountController : BaseApiController
{
    private readonly SignInManager<AppUser> _signInManager;
    private readonly UserManager<AppUser> _userManager;

    public AccountController(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager)
    {
        _signInManager = signInManager;
        _userManager = userManager;
    }

    [HttpPost("login")]
    public async Task<ActionResult<UserDto>> Login(LoginDto loginDto)
    {
        var user = await _userManager.FindByEmailAsync(loginDto.Email);

        if (user == null)
            return Unauthorized(new ApiResponse((int)HttpStatusCode.Unauthorized));

        var result = await _signInManager
            .CheckPasswordSignInAsync(user, loginDto.Password, false);

        if (!result.Succeeded)
            return Unauthorized(new ApiResponse((int)HttpStatusCode.Unauthorized));

        return new UserDto
        {
            DisplayName = user.DisplayName,
            Email = user.Email,
            Token = "This will have a token"
        };
    }

    [HttpPost("register")]
    public async Task<ActionResult<UserDto>> Register(RegisterDto registerDto)
    {
        var user = new AppUser
        {
            DisplayName = registerDto.DisplayName,
            Email = registerDto.Email,
            UserName = registerDto.Email
        };

        var result = await _userManager.CreateAsync(user, registerDto.Password);

        if (!result.Succeeded)
            return BadRequest(new ApiResponse((int)HttpStatusCode.BadRequest));

        return new UserDto
        {
            DisplayName = user.DisplayName,
            Email = user.Email,
            Token = "This will have a token"
        };
    }
}