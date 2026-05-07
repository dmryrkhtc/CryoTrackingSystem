using Microsoft.AspNetCore.Mvc;
using CryoTracking.Application.DTOs.Auth;
using CryoTracking.Domain.Response;
using CryoTracking.Infrastructure.Repositories;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IAuthRepository _authRepository;

    public AuthController(IAuthRepository authRepository)
    {
        _authRepository = authRepository;
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginRequestDto dto)
    {
        try
        {
           
            var result = await _authRepository.LoginAsync(dto);

            if (result.Success)
                return Ok(result);

            return BadRequest(result);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Detaylı Hata: {ex.Message} --- İç Hata: {ex.InnerException?.Message}");
        }
    }
}
