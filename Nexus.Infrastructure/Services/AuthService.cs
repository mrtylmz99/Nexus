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

    public AuthService(NexusDbContext context, IConfiguration configuration)
    {
        _context = context;
        _configuration = configuration;
    }

    public async Task<AuthResponseDto> LoginAsync(LoginDto loginDto)
    {
        // Find user by email / Kullanıcıyı e-posta ile bul
        var user = await _context.Users
            .FirstOrDefaultAsync(u => u.Email == loginDto.Email);

        // Verify user existence and password hash / Kullanıcı varlığını ve şifre hash'ini doğrula
        if (user == null || !BCrypt.Net.BCrypt.Verify(loginDto.Password, user.PasswordHash))
        {
            throw new Exception("Invalid credentials");
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
                new Claim(ClaimTypes.Name, user.Username)
            }),
            Expires = DateTime.UtcNow.AddDays(7),
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
        };

        var token = tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(token);
    }
}
