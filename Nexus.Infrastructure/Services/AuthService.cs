using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using BCrypt.Net;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Nexus.Application.DTOs.Auth;
using Nexus.Application.DTOs.User;
using Nexus.Application.Interfaces;
using Nexus.Domain.Entities;
using Nexus.Infrastructure.Persistence;

namespace Nexus.Infrastructure.Services;

public class AuthService : IAuthService
{
    private readonly NexusDbContext _context;
    private readonly IConfiguration _configuration;
    private readonly IEmailService _emailService;

    public AuthService(NexusDbContext context, IConfiguration configuration, IEmailService emailService)
    {
        _context = context;
        _configuration = configuration;
        _emailService = emailService;
    }

    public async Task<AuthResponseDto> LoginAsync(LoginDto loginDto)
    {
        // Find user by email / Kullanıcıyı e-posta ile bul
        var user = await _context.Users
            .FirstOrDefaultAsync(u => u.Email == loginDto.Email);

        // Verify user existence, password hash, and status / Kullanıcı durumu kontrolü
        if (user == null || !BCrypt.Net.BCrypt.Verify(loginDto.Password, user.PasswordHash))
        {
            throw new Exception("Invalid credentials");
        }

        if (user.Status != Domain.Enums.UserStatus.Active)
        {
            throw new Exception("Account is not active. Please contact support.");
        }

        // Generate Token / Token Üret
        var token = GenerateJwtToken(user);

        return new AuthResponseDto
        {
            Token = token,
            User = new UserDto
            {
                Id = user.Id,
                Username = user.Username,
                Email = user.Email,
                FullName = user.FullName,
                CreatedAt = user.CreatedAt
            }
        };
    }

    public async Task<AuthResponseDto> RegisterAsync(RegisterDto registerDto)
    {
        if (await _context.Users.AnyAsync(u => u.Email == registerDto.Email))
        {
            throw new Exception("Email already exists");
        }

        // Hash the password before saving / Kaydetmeden önce şifreyi hash'le
        var passwordHash = BCrypt.Net.BCrypt.HashPassword(registerDto.Password);

        var user = new User
        {
            Username = registerDto.Username,
            Email = registerDto.Email,
            FullName = registerDto.FullName,
            PasswordHash = passwordHash,
            // Generate default avatar using UI Avatars (initials based)
            ProfilePictureUrl = $"https://ui-avatars.com/api/?name={Uri.EscapeDataString(registerDto.FullName)}&background=random&color=fff",
            Status = Domain.Enums.UserStatus.Active,
            CreatedAt = DateTime.UtcNow
        };

        _context.Users.Add(user);
        await _context.SaveChangesAsync();

        var token = GenerateJwtToken(user);

        return new AuthResponseDto
        {
            Token = token,
            User = new UserDto
            {
                Id = user.Id,
                Username = user.Username,
                Email = user.Email,
                FullName = user.FullName,
                CreatedAt = user.CreatedAt
            }
        };
    }

    public async Task ForgotPasswordAsync(string email)
    {
        var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
        if (user == null)
        {
            // Security: Don't reveal exact user existence, but for now we just return
            // Güvenlik: Kullanıcının varlığını belli etme, şimdilik sadece dönüyoruz
            return;
        }

        // Generate 6-digit code / 6 haneli kod üret
        var code = new Random().Next(100000, 999999).ToString();
        user.ResetCode = code;
        user.ResetCodeExpires = DateTime.UtcNow.AddMinutes(15); // Valid for 15 mins

        await _context.SaveChangesAsync();

        // Send Email (Mock) / E-posta Gönder (Mock)
        await _emailService.SendEmailAsync(user.Email, "Nexus Password Reset Code", $"Your verification code is: {code}");
    }

    public async Task<bool> VerifyResetCodeAsync(string email, string code)
    {
        var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
        if (user == null || user.ResetCode != code || user.ResetCodeExpires < DateTime.UtcNow)
        {
            return false;
        }
        return true;
    }

    public async Task ResetPasswordAsync(string email, string code, string newPassword)
    {
        var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
        if (user == null || user.ResetCode != code || user.ResetCodeExpires < DateTime.UtcNow)
        {
            throw new Exception("Invalid or expired reset code");
        }

        // Hash new password / Yeni şifreyi hash'le
        user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(newPassword);
        
        // Clear reset code / Reset kodunu temizle
        user.ResetCode = null;
        user.ResetCodeExpires = null;

        await _context.SaveChangesAsync();
    }

    private string GenerateJwtToken(User user)
    {
        var jwtSettings = _configuration.GetSection("JwtSettings");
        var secretKey = jwtSettings["SecretKey"];

        if (string.IsNullOrEmpty(secretKey))
        {
            throw new Exception("JwtSettings:SecretKey is missing in configuration.");
        }

        var key = Encoding.ASCII.GetBytes(secretKey);

        var tokenHandler = new JwtSecurityTokenHandler();
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Name, user.Username),
                new Claim(ClaimTypes.Role, user.Role.ToString())
            }),
            Expires = DateTime.UtcNow.AddDays(7),
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
        };

        var token = tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(token);
    }
}
