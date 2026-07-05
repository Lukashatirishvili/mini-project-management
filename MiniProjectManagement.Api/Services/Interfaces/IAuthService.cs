using MiniProjectManagement.Api.DTOs.Auth;
using MiniProjectManagement.Api.Helpers;

namespace MiniProjectManagement.Api.Services.Interfaces;

public interface IAuthService
{
    Task<ServiceResult<RegisterResponseDto>> RegisterAsync(RegisterDto dto);
}