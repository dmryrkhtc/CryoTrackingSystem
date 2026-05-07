using CryoTracking.Application.DTOs.Auth;
using CryoTracking.Application.Interfaces;
using CryoTracking.Domain.Response;
using CryoTracking.Domain.Settings;
using CryoTracking.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace CryoTracking.Infrastructure.Repositories;

public class AuthRepository : IAuthRepository
{
    private readonly CryoDbContext _context;
    private readonly JwtSettings _jwtSettings;

    public AuthRepository(CryoDbContext context, IOptions<JwtSettings> jwtSettings)
    {
        _context = context;
        _jwtSettings = jwtSettings.Value;
    }

    public async Task<ResultResponse<LoginResponseDto>> LoginAsync(LoginRequestDto dto)
    {
        // Kullanıcıyı ve Rolünü bul
        var user = await _context.Users
            .Include(u => u.Role)
            .FirstOrDefaultAsync(u => u.Email == dto.Email);

        // BCrypt ile şifre doğrulaması yap
        if (user == null || !BCrypt.Net.BCrypt.Verify(dto.Password, user.Password))
        {
            return new ResultResponse<LoginResponseDto> { Success = false, Message = "E-posta veya şifre hatalı!" };
        }

        // 3. JWT Üretimi
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.UTF8.GetBytes(_jwtSettings.Secret);

        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, user.UserId.ToString()),
            new Claim(ClaimTypes.Name, user.Name),
            new Claim(ClaimTypes.Email, user.Email),
            new Claim(ClaimTypes.Role, user.Role.RoleName) // Yetkilendirme (Authorization) için şart!
        };

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),
            Expires = DateTime.UtcNow.AddMinutes(_jwtSettings.ExpiryMinutes),
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature),
            Issuer = _jwtSettings.Issuer,
            Audience = _jwtSettings.Audience
        };

        var token = tokenHandler.CreateToken(tokenDescriptor);

        return new ResultResponse<LoginResponseDto>
        {
            Success = true,
            Data = new LoginResponseDto
            {
                Token = tokenHandler.WriteToken(token),
                Name = user.Name,
                Role = user.Role.RoleName
            }
        };
    }
}