using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MiniProjectManagement.Api.DTOs.Auth;
using MiniProjectManagement.Api.Models;
using MiniProjectManagement.Api.Services.Interfaces;

namespace MiniProjectManagement.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : BaseApiController
{
    private readonly IAuthService _authService;

    public AuthController(IAuthService authService)
    {
        _authService = authService;
    }

    [HttpPost("register")]
    public async Task<ActionResult<RegisterResponseDto>> Register(RegisterDto dto)
    {
        var result = await _authService.RegisterAsync(dto);

        if (!result.Succeeded)
        {
            return HandleServiceError<RegisterResponseDto>(result);
        }

        return CreatedAtAction(nameof(Register), new { id = result.Data!.UserId }, result.Data);
    }

    [HttpPost("login")]
    public async Task<ActionResult<AuthResponseDto>> Login(LoginDto dto)
    {
        var result = await _authService.LoginAsync(dto);

        if (!result.Succeeded)
        {
            return HandleServiceError<AuthResponseDto>(result);
        }
        
        return Ok(result.Data);
    }
    
    [Authorize]
    [HttpGet("me")]
    public ActionResult<CurrentUserDto> GetCurrentUser()
    {
        var userIdValue = User.FindFirstValue(ClaimTypes.NameIdentifier);

        if (!int.TryParse(userIdValue, out var userId))
        {
            return Unauthorized();
        }

        var fullName = User.FindFirstValue(ClaimTypes.Name) ?? string.Empty;
        var email = User.FindFirstValue(ClaimTypes.Email) ?? string.Empty;
        var roleValue = User.FindFirstValue(ClaimTypes.Role) ?? string.Empty;

        if (!Enum.TryParse<UserRole>(roleValue, out var role))
        {
            return Unauthorized();
        }

        var currentUser = new CurrentUserDto
        {
            UserId = userId,
            FullName = fullName,
            Email = email,
            Role =  role
        };

        return Ok(currentUser);
    }
}
