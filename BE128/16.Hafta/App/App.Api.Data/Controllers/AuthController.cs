using App.Models.DTO.Auth;
using App.Services.Abstract;
using Ardalis.Result;
using Ardalis.Result.AspNetCore;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace App.Api.Data.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class AuthController(IAuthService authService) : ControllerBase
    {
        [HttpPost("login")]
        [TranslateResultToActionResult]
        [AllowAnonymous]
        public async Task<Result<AuthLoginResult>> Login(AuthLoginRequest loginRequest)
        {
            return await authService.LoginAsync(loginRequest);
        }

        [HttpPost("forgot-password")]
        [TranslateResultToActionResult]
        [AllowAnonymous]
        public async Task<Result<AuthLoginResult>> ForgotPassword(AuthForgotPasswordRequest forgotPasswordRequest)
        {
            return await authService.ForgotPasswordAsync(forgotPasswordRequest);
        }

        [HttpPost("register")]
        [TranslateResultToActionResult]
        [AllowAnonymous]
        public async Task<Result> Register(AuthRegisterRequest registerRequest)
        {
            return await authService.RegisterAsync(registerRequest);
        }

        [HttpPost("refresh-token")]
        [TranslateResultToActionResult]
        [AllowAnonymous]
        public async Task<Result<AuthRefreshTokenResult>> RefreshToken(AuthRefreshTokenRequest refreshTokenRequest)
        {
            return await authService.RefreshTokenAsync(refreshTokenRequest);
        }

        [HttpPost("reset-password")]
        [TranslateResultToActionResult]
        [AllowAnonymous]
        public async Task<Result> ResetPassword(AuthResetPasswordRequest resetPasswordRequest)
        {
            return await authService.ResetPasswordAsync(resetPasswordRequest);
        }
    }
}
