using Microsoft.AspNetCore.Mvc;
using MiniProjectManagement.Api.DTOs.Auth;
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
}
