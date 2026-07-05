using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using MiniProjectManagement.Api.Data;
using MiniProjectManagement.Api.DTOs.Auth;
using MiniProjectManagement.Api.Helpers;
using MiniProjectManagement.Api.Models;
using MiniProjectManagement.Api.Services.Interfaces;

namespace MiniProjectManagement.Api.Services;

public class AuthService : IAuthService
{
    private readonly AppDbContext _context;
    private readonly IPasswordHasher<User> _passwordHasher;
    private readonly ILogger<AuthService> _logger;

    public AuthService(AppDbContext context, IPasswordHasher<User> passwordHasher, ILogger<AuthService> logger)
    {
        _context = context;
        _passwordHasher = passwordHasher;
        _logger = logger;
    }
    
    public async Task<ServiceResult<RegisterResponseDto>> RegisterAsync(RegisterDto dto)
    {
        var normalizedEmail = dto.Email.Trim().ToLower();

        var emailExists = await _context.Users.AnyAsync(u => u.Email == normalizedEmail);

        if (emailExists)
        {
            _logger.LogWarning("Registration failed. Email already exists: {Email}", normalizedEmail);
            return ServiceResult<RegisterResponseDto>.Conflict("Email is Already Registered");
        }

        var user = new User
        {
            FullName = dto.FullName,
            Email = normalizedEmail,
            Role = UserRole.User,
            CreatedAt = DateTime.UtcNow
        };
        
        user.PasswordHash = _passwordHasher.HashPassword(user, dto.Password);
        
        _context.Users.Add(user);
        await _context.SaveChangesAsync();
        
        _logger.LogInformation("User registered. UserId: {UserId}", user.Id);

        var response = new RegisterResponseDto
        {
            UserId = user.Id,
            FullName = user.FullName,
            Email = user.Email,
            Role = user.Role,
        };
        
        return ServiceResult<RegisterResponseDto>.Success(response);
    }
}