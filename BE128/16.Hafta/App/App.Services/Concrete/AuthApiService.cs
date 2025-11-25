using App.Models.DTO.Auth;
using App.Services.Abstract;
using Ardalis.Result;
using System.Net;
using System.Net.Http.Json;

namespace App.Services.Concrete
{
    public class AuthApiService(IServiceProvider serviceProvider) : AppServiceBase(serviceProvider), IAuthService
    {
        private HttpClient Client => GetRequiredService<IHttpClientFactory>()
            .CreateClient("Api.Data");

        public async Task<Result> ForgotPasswordAsync(AuthForgotPasswordRequest forgotPasswordRequest)
        {
            var validationResult = await ValidateModelAsync(forgotPasswordRequest);
            if (!validationResult.IsSuccess)
            {
                return validationResult;
            }

            var response = await Client.PostAsJsonAsync("auth/forgot-password", forgotPasswordRequest);

            if (response.StatusCode == HttpStatusCode.NotFound)
            {
                return Result.NotFound();
            }

            if (response.StatusCode == HttpStatusCode.BadRequest)
            {
                return Result.Invalid();
            }

            if (!response.IsSuccessStatusCode)
            {
                return Result.Unavailable();
            }

            return Result.Success();
        }

        public async Task<Result<AuthLoginResult>> LoginAsync(AuthLoginRequest loginRequest)
        {
            var validationResult = await ValidateModelAsync(loginRequest);
            if (!validationResult.IsSuccess)
            {
                return validationResult;
            }

            var response = await Client.PostAsJsonAsync("auth/login", loginRequest);

            if (response.StatusCode == HttpStatusCode.NotFound)
            {
                return Result.NotFound();
            }

            if (response.StatusCode == HttpStatusCode.BadRequest)
            {
                return Result.Invalid();
            }

            if (!response.IsSuccessStatusCode)
            {
                return Result.Unavailable();
            }

            var loginResult = await response.Content.ReadFromJsonAsync<AuthLoginResult>();
            if (loginResult is null)
            {
                return Result.Unavailable();
            }

            return Result.Success(loginResult);
        }

        public async Task<Result<AuthRefreshTokenResult>> RefreshTokenAsync(AuthRefreshTokenRequest refreshTokenRequest)
        {
            var validationResult = await ValidateModelAsync(refreshTokenRequest);
            if (!validationResult.IsSuccess)
            {
                return validationResult;
            }

            var response = await Client.PostAsJsonAsync("auth/refresh-token", refreshTokenRequest);

            if (response.StatusCode == HttpStatusCode.NotFound)
            {
                return Result.NotFound();
            }

            if (response.StatusCode == HttpStatusCode.BadRequest)
            {
                return Result.Invalid();
            }

            if (!response.IsSuccessStatusCode)
            {
                return Result.Unavailable();
            }

            var refreshTokenResult = await response.Content.ReadFromJsonAsync<AuthRefreshTokenResult>();
            if (refreshTokenResult is null)
            {
                return Result.Unavailable();
            }

            return Result.Success(refreshTokenResult);
        }

        public async Task<Result> RegisterAsync(AuthRegisterRequest registerRequest)
        {
            var validationResult = await ValidateModelAsync(registerRequest);
            if (!validationResult.IsSuccess)
            {
                return validationResult;
            }

            var response = await Client.PostAsJsonAsync("auth/register", registerRequest);

            if (response.StatusCode == HttpStatusCode.NotFound)
            {
                return Result.NotFound();
            }

            if (response.StatusCode == HttpStatusCode.BadRequest)
            {
                return Result.Invalid();
            }

            if (!response.IsSuccessStatusCode)
            {
                return Result.Unavailable();
            }

            return Result.Success();
        }

        public async Task<Result> ResetPasswordAsync(AuthResetPasswordRequest resetPasswordRequest)
        {
            var validationResult = await ValidateModelAsync(resetPasswordRequest);
            if (!validationResult.IsSuccess)
            {
                return validationResult;
            }

            var response = await Client.PostAsJsonAsync("auth/reset-password", resetPasswordRequest);

            if (response.StatusCode == HttpStatusCode.NotFound)
            {
                return Result.NotFound();
            }

            if (response.StatusCode == HttpStatusCode.BadRequest)
            {
                return Result.Invalid();
            }

            if (!response.IsSuccessStatusCode)
            {
                return Result.Unavailable();
            }

            return Result.Success();
        }
    }
}
