using Nexus.Application.DTOs.Auth;

namespace Nexus.Application.Interfaces;

public interface IAuthService
{
    Task<AuthResponseDto> LoginAsync(LoginDto loginDto);
    Task<AuthResponseDto> RegisterAsync(RegisterDto registerDto);
    Task ForgotPasswordAsync(string email);
    Task<bool> VerifyResetCodeAsync(string email, string code);
    Task ResetPasswordAsync(string email, string code, string newPassword);
}
