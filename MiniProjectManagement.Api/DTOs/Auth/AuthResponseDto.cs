using MiniProjectManagement.Api.Models;

namespace MiniProjectManagement.Api.DTOs.Auth;

public class AuthResponseDto
{
    public int UserId { get; set; }

    public string FullName { get; set; } = string.Empty;

    public string Email { get; set; } = string.Empty;

    public UserRole Role { get; set; }

    public string Token { get; set; } = string.Empty;
}