using Ardalis.Result;
using App.Models.DTO;
using App.Services.Abstract;
using System.Net.Http;
using System.Net.Http.Json;

namespace App.Services.Concrete;

public class ProductService : IProductService
{
    private readonly HttpClient client;
    public ProductService(IHttpClientFactory httpClientFactory)
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

    public async Task<Result<List<ProductDto>>> ListProductsAsync(string jwt)
    {
        var response = await SendApiRequestAsync("api/product/list", HttpMethod.Get, jwt);
        if (!response.IsSuccessStatusCode)
        {
            if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                return Result.Unauthorized();
            return Result.Error("Ürünler alýnamadý.");
        }
        var products = await response.Content.ReadFromJsonAsync<List<ProductDto>>();
        return Result.Success(products ?? new List<ProductDto>());
    }

    public async Task<Result<ProductDetailDto>> GetProductDetailAsync(string jwt, int productId)
    {
        var response = await SendApiRequestAsync($"api/product/{productId}", HttpMethod.Get, jwt);
        if (!response.IsSuccessStatusCode)
        {
            if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                return Result.Unauthorized();
            if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
                return Result.NotFound();
            return Result.Error("Ürün detayý alýnamadý.");
        }
        var detail = await response.Content.ReadFromJsonAsync<ProductDetailDto>();
        return Result.Success(detail!);
    }

    public async Task<Result<CreateProductResponseDto>> CreateProductAsync(string jwt, CreateProductRequestDto request)
    {
        var response = await SendApiRequestAsync("api/product", HttpMethod.Post, jwt, request);
        if (!response.IsSuccessStatusCode)
        {
            if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                return Result.Unauthorized();
            return Result.Error("Ürün oluþturulamadý.");
        }
        var created = await response.Content.ReadFromJsonAsync<CreateProductResponseDto>();
        return Result.Success(created!);
    }
}
