using MiniProjectManagement.Api.Models;

namespace MiniProjectManagement.Api.DTOs.Auth;

public class CurrentUserDto
{
    public int UserId { get; set; }

    public string FullName { get; set; } = string.Empty;

    public string Email { get; set; } = string.Empty;

    public UserRole Role { get; set; }
}