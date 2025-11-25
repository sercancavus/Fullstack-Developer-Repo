using Ardalis.Result;
using App.Models.DTO;
using App.Services.Abstract;
using System.Net.Http;
using System.Net.Http.Json;

namespace App.Services.Concrete;

public class UserService : IUserService
{
    private readonly HttpClient client;
    public UserService(IHttpClientFactory httpClientFactory)
    {
        client = httpClientFactory.CreateClient("DataApi");
    }

    protected async Task<HttpResponseMessage> SendApiRequestAsync(string apiRoute, HttpMethod method, string jwt, object? payload = null)
    {
        var httpRequestMessage = new HttpRequestMessage(method, apiRoute);
        if (!string.IsNullOrWhiteSpace(jwt))
            httpRequestMessage.Headers.Add("Authorization", $"Bearer {jwt}");
        if (payload is not null)
        {
            httpRequestMessage.Content = JsonContent.Create(payload);
        }
        return await client.SendAsync(httpRequestMessage);
    }

    public async Task<Result<List<UserDto>>> ListUsersAsync(string jwt)
    {
        var response = await SendApiRequestAsync("api/admin/users", HttpMethod.Get, jwt);
        if (!response.IsSuccessStatusCode)
        {
            if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                return Result.Unauthorized();
            return Result.Error("Kullanýcýlar alýnamadý.");
        }
        var users = await response.Content.ReadFromJsonAsync<List<UserDto>>();
        return Result.Success(users ?? new List<UserDto>());
    }

    public async Task<Result> EnableUserAsync(string jwt, int userId)
    {
        var response = await SendApiRequestAsync($"api/admin/users/{userId}/enable", HttpMethod.Post, jwt);
        if (!response.IsSuccessStatusCode)
        {
            if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                return Result.Unauthorized();
            if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
                return Result.NotFound();
            return Result.Error("Kullanýcý aktif edilemedi.");
        }
        return Result.Success();
    }

    public async Task<Result> DisableUserAsync(string jwt, int userId)
    {
        var response = await SendApiRequestAsync($"api/admin/users/{userId}/disable", HttpMethod.Post, jwt);
        if (!response.IsSuccessStatusCode)
        {
            if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                return Result.Unauthorized();
            if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
                return Result.NotFound();
            return Result.Error("Kullanýcý pasif edilemedi.");
        }
        return Result.Success();
    }

    public async Task<Result> ApproveSellerRequestAsync(string jwt, int userId)
    {
        var response = await SendApiRequestAsync($"api/admin/users/{userId}/approve-seller", HttpMethod.Post, jwt);
        if (!response.IsSuccessStatusCode)
        {
            if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                return Result.Unauthorized();
            if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
                return Result.NotFound();
            return Result.Error("Satýcý talebi onaylanamadý.");
        }
        return Result.Success();
    }
}
