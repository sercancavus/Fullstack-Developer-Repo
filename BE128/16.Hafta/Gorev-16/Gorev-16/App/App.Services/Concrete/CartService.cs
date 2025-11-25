using Ardalis.Result;
using App.Models.DTO;
using App.Services.Abstract;
using System.Net.Http;
using System.Net.Http.Json;

namespace App.Services.Concrete;

public class CartService : ICartService
{
    private readonly HttpClient client;
    public CartService(IHttpClientFactory httpClientFactory)
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

    public async Task<Result<List<CartItemDto>>> GetMyCartAsync(string jwt)
    {
        var response = await SendApiRequestAsync("api/cart/my", HttpMethod.Get, jwt);
        if (!response.IsSuccessStatusCode)
        {
            if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                return Result.Unauthorized();
            return Result.Error("Sepet alýnamadý.");
        }
        var items = await response.Content.ReadFromJsonAsync<List<CartItemDto>>();
        return Result.Success(items ?? new List<CartItemDto>());
    }

    public async Task<Result<CartSummaryDto>> GetCartSummaryAsync(string jwt)
    {
        var response = await SendApiRequestAsync("api/cart/summary", HttpMethod.Get, jwt);
        if (!response.IsSuccessStatusCode)
        {
            if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                return Result.Unauthorized();
            return Result.Error("Sepet özeti alýnamadý.");
        }
        var summary = await response.Content.ReadFromJsonAsync<CartSummaryDto>();
        return Result.Success(summary!);
    }

    public async Task<Result> AddToCartAsync(string jwt, int productId)
    {
        var response = await SendApiRequestAsync($"api/cart/add/{productId}", HttpMethod.Post, jwt);
        if (!response.IsSuccessStatusCode)
        {
            if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                return Result.Unauthorized();
            if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
                return Result.NotFound();
            return Result.Error("Ürün sepete eklenemedi.");
        }
        return Result.Success();
    }

    public async Task<Result> RemoveFromCartAsync(string jwt, int cartItemId)
    {
        var response = await SendApiRequestAsync($"api/cart/{cartItemId}", HttpMethod.Delete, jwt);
        if (!response.IsSuccessStatusCode)
        {
            if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                return Result.Unauthorized();
            if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
                return Result.NotFound();
            return Result.Error("Ürün sepetten çýkarýlamadý.");
        }
        return Result.Success();
    }

    public async Task<Result<CartItemDto>> UpdateCartItemAsync(string jwt, UpdateCartItemRequestDto updateRequest)
    {
        var response = await SendApiRequestAsync($"api/cart/{updateRequest.CartItemId}", HttpMethod.Put, jwt, updateRequest);
        if (!response.IsSuccessStatusCode)
        {
            if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                return Result.Unauthorized();
            if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
                return Result.NotFound();
            return Result.Error("Sepet güncellenemedi.");
        }
        var item = await response.Content.ReadFromJsonAsync<CartItemDto>();
        return Result.Success(item!);
    }
}
