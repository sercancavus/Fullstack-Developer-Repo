using Ardalis.Result;
using App.Models.DTO;
using App.Services.Abstract;
using System.Net.Http;
using System.Net.Http.Json;

namespace App.Services.Concrete;

public class CategoryService : ICategoryService
{
    private readonly HttpClient client;
    public CategoryService(IHttpClientFactory httpClientFactory)
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

    public async Task<Result<List<CategoryDto>>> ListCategoriesAsync(string jwt)
    {
        var response = await SendApiRequestAsync("api/category", HttpMethod.Get, jwt);
        if (!response.IsSuccessStatusCode)
        {
            if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                return Result.Unauthorized();
            return Result.Error("Kategoriler alýnamadý.");
        }
        var categories = await response.Content.ReadFromJsonAsync<List<CategoryDto>>();
        return Result.Success(categories ?? new List<CategoryDto>());
    }

    public async Task<Result<CategoryDto>> GetCategoryAsync(string jwt, int categoryId)
    {
        var response = await SendApiRequestAsync($"api/category/{categoryId}", HttpMethod.Get, jwt);
        if (!response.IsSuccessStatusCode)
        {
            if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                return Result.Unauthorized();
            if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
                return Result.NotFound();
            return Result.Error("Kategori alýnamadý.");
        }
        var category = await response.Content.ReadFromJsonAsync<CategoryDto>();
        return Result.Success(category!);
    }

    public async Task<Result<CreateCategoryResponseDto>> CreateCategoryAsync(string jwt, CreateCategoryRequestDto request)
    {
        var response = await SendApiRequestAsync("api/category", HttpMethod.Post, jwt, request);
        if (!response.IsSuccessStatusCode)
        {
            if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                return Result.Unauthorized();
            return Result.Error("Kategori oluþturulamadý.");
        }
        var created = await response.Content.ReadFromJsonAsync<CreateCategoryResponseDto>();
        return Result.Success(created!);
    }

    public async Task<Result> UpdateCategoryAsync(string jwt, int categoryId, UpdateCategoryRequestDto request)
    {
        var response = await SendApiRequestAsync($"api/category/{categoryId}", HttpMethod.Put, jwt, request);
        if (!response.IsSuccessStatusCode)
        {
            if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                return Result.Unauthorized();
            if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
                return Result.NotFound();
            return Result.Error("Kategori güncellenemedi.");
        }
        return Result.Success();
    }

    public async Task<Result> DeleteCategoryAsync(string jwt, int categoryId)
    {
        var response = await SendApiRequestAsync($"api/category/{categoryId}", HttpMethod.Delete, jwt);
        if (!response.IsSuccessStatusCode)
        {
            if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                return Result.Unauthorized();
            if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
                return Result.NotFound();
            return Result.Error("Kategori silinemedi.");
        }
        return Result.Success();
    }
}
