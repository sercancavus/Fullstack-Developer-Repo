using Ardalis.Result;
using App.Models.DTO;
using App.Services.Abstract;
using System.Net.Http;
using System.Net.Http.Json;

namespace App.Services.Concrete;

public class ProfileService : IProfileService
{
    private readonly HttpClient client;
    public ProfileService(IHttpClientFactory httpClientFactory)
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

    public async Task<Result<ProfileDetailsDto>> GetProfileAsync(string jwt)
    {
        var response = await SendApiRequestAsync("api/profile/me", HttpMethod.Get, jwt);
        if (!response.IsSuccessStatusCode)
        {
            if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                return Result.Unauthorized();
            return Result.Error("Profil alýnamadý.");
        }
        var profile = await response.Content.ReadFromJsonAsync<ProfileDetailsDto>();
        return Result.Success(profile!);
    }

    public async Task<Result> UpdateProfileAsync(string jwt, UpdateProfileRequestDto updateRequest)
    {
        var response = await SendApiRequestAsync("api/profile", HttpMethod.Put, jwt, updateRequest);
        if (!response.IsSuccessStatusCode)
        {
            if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                return Result.Unauthorized();
            return Result.Error("Profil güncellenemedi.");
        }
        return Result.Success();
    }
}
