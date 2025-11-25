using Ardalis.Result;
using App.Models.DTO;
using App.Services.Abstract;
using System.Net.Http;
using System.Net.Http.Json;

namespace App.Services.Concrete;

public class AuthService : IAuthService
{
    private readonly HttpClient client;
    public AuthService(IHttpClientFactory httpClientFactory)
    {
        client = httpClientFactory.CreateClient("DataApi");
    }

    public async Task<Result<LoginResponseDto>> LoginAsync(LoginRequestDto loginRequest)
    {
        var response = await client.PostAsJsonAsync("api/auth/login", loginRequest);
        if (!response.IsSuccessStatusCode)
        {
            if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
                return Result.NotFound();
            if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                return Result.Unauthorized();
            return Result.Error("Giriþ baþarýsýz.");
        }
        var loginResponse = await response.Content.ReadFromJsonAsync<LoginResponseDto>();
        return Result.Success(loginResponse!);
    }

    public async Task<Result<RegisterResponseDto>> RegisterAsync(RegisterRequestDto registerRequest)
    {
        var response = await client.PostAsJsonAsync("api/auth/register", registerRequest);
        if (!response.IsSuccessStatusCode)
        {
            if (response.StatusCode == System.Net.HttpStatusCode.Conflict)
                return Result.Error("Bu e-posta ile bir kullanýcý zaten mevcut.");
            return Result.Error("Kayýt baþarýsýz.");
        }
        var registerResponse = await response.Content.ReadFromJsonAsync<RegisterResponseDto>();
        return Result.Success(registerResponse!);
    }
}
