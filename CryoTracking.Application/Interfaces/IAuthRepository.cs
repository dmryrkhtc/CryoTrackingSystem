using CryoTracking.Application.DTOs.Auth;
using CryoTracking.Domain.Response;


public interface IAuthRepository
{
    Task<ResultResponse<LoginResponseDto>> LoginAsync(LoginRequestDto dto);
}