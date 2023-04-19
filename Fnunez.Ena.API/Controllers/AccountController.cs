using System.Net;
using AutoMapper;
using Fnunez.Ena.API.Dtos;
using Fnunez.Ena.API.Errors;
using Fnunez.Ena.API.Extensions;
using Fnunez.Ena.Core.Entities.Identity;
using Fnunez.Ena.Core.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Fnunez.Ena.API.Controllers;

public class AccountController : BaseApiController
{
    private readonly SignInManager<AppUser> _signInManager;
    private readonly UserManager<AppUser> _userManager;
    private readonly ITokenService _tokenService;
    private readonly IMapper _mapper;

    public AccountController(
        SignInManager<AppUser> signInManager,
        UserManager<AppUser> userManager,
        ITokenService tokenService,
        IMapper mapper)
    {
        _signInManager = signInManager;
        _userManager = userManager;
        _tokenService = tokenService;
        _mapper = mapper;
    }

    [Authorize]
    [HttpGet("getcurrentuser")]
    public async Task<ActionResult<UserDto>> GetCurrentUser()
    {
        AppUser user = await _userManager
            .FindUserByEmailFromClaimsPrincipalAsync(HttpContext.User);

        return new UserDto
        {
            DisplayName = user.DisplayName,
            Email = user.Email,
            Token = _tokenService.CreateToken(user)
        };
    }

    [HttpGet("emailexists")]
    public async Task<ActionResult<bool>> EmailExists([FromQuery] string email)
    {
        return await _userManager.FindByEmailAsync(email) != null;
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
            Token = _tokenService.CreateToken(user)
        };
    }

    [HttpPost("register")]
    public async Task<ActionResult<UserDto>> Register(RegisterDto registerDto)
    {
        if (EmailExists(registerDto.Email).Result.Value)
        {
            return new BadRequestObjectResult(
                new ApiValidationErrorResponse
                {
                    Errors = new[] { "Email address is in user" }
                }
            );
        }

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
            Token = _tokenService.CreateToken(user)
        };
    }

    [Authorize]
    [HttpGet("address")]
    public async Task<ActionResult<AddressDto>> GetUserAddress()
    {
        AppUser user = await _userManager
            .FindUserByEmailFromClaimsPrincipalWithAddressAsync(HttpContext.User);

        return _mapper.Map<Address, AddressDto>(user.Address);
    }

    [Authorize]
    [HttpPut("address")]
    public async Task<ActionResult<AddressDto>> UpdateUserAddress(AddressDto addressDto)
    {
        AppUser user = await _userManager
            .FindUserByEmailFromClaimsPrincipalWithAddressAsync(HttpContext.User);

        user.Address = _mapper.Map<AddressDto, Address>(addressDto);

        var results = await _userManager.UpdateAsync(user);

        if (!results.Succeeded)
            return BadRequest("Wrongs updating User's info");

        return Ok(_mapper.Map<Address, AddressDto>(user.Address));
    }
}