using Ardalis.Result;
using App.Models.DTO;

namespace App.Services.Abstract;

public interface IAuthService
{
    Task<Result<LoginResponseDto>> LoginAsync(LoginRequestDto loginRequest);
    Task<Result<RegisterResponseDto>> RegisterAsync(RegisterRequestDto registerRequest);
}
