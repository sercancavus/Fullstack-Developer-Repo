using App.Models.DTO.Auth;
using Ardalis.Result;

namespace App.Services.Abstract
{
    public interface IAuthService
    {
        Task<Result<AuthLoginResult>> LoginAsync(AuthLoginRequest loginRequest);
        Task<Result<AuthRefreshTokenResult>> RefreshTokenAsync(AuthRefreshTokenRequest refreshTokenRequest);
        Task<Result> RegisterAsync(AuthRegisterRequest registerRequest);
        Task<Result> ForgotPasswordAsync(AuthForgotPasswordRequest forgotPasswordRequest);
        Task<Result> ResetPasswordAsync(AuthResetPasswordRequest resetPasswordRequest);
    }
}
