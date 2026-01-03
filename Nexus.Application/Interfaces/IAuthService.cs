using Nexus.Application.DTOs.Auth;

namespace Nexus.Application.Interfaces;

public interface IAuthService
{
    Task<AuthResponseDto> LoginAsync(LoginDto loginDto);
    Task<AuthResponseDto> RegisterAsync(RegisterDto registerDto);
}
