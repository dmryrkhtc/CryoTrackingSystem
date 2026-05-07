namespace CryoTracking.Application.DTOs.Auth;
public class LoginResponseDto
{
    public string Token { get; set; } = null!;
    public string Name { get; set; } = null!;
    public string Role { get; set; } = null!;
}