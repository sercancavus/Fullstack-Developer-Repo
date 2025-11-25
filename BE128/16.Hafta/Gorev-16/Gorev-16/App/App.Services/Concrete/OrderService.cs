using Ardalis.Result;
using App.Models.DTO;
using App.Services.Abstract;
using System.Net.Http;
using System.Net.Http.Json;

namespace App.Services.Concrete;

public class OrderService : IOrderService
{
    private readonly HttpClient _client;
    public OrderService(IHttpClientFactory httpClientFactory)
    {
        _client = httpClientFactory.CreateClient("DataApi");
    }

    private async Task<HttpResponseMessage> SendAsync(string route, HttpMethod method, string jwt, object? payload = null)
    {
        var msg = new HttpRequestMessage(method, route);
        if (!string.IsNullOrWhiteSpace(jwt)) msg.Headers.Add("Authorization", $"Bearer {jwt}");
        if (payload is not null) msg.Content = JsonContent.Create(payload);
        return await _client.SendAsync(msg);
    }

    public async Task<Result<NewOrderResponseDto>> PlaceOrderAsync(string jwt, PlaceOrderRequestDto request)
    {
        var resp = await SendAsync("api/order", HttpMethod.Post, jwt, request);
        if (!resp.IsSuccessStatusCode)
        {
            if (resp.StatusCode == System.Net.HttpStatusCode.Unauthorized) return Result.Unauthorized();
            return Result.Error("Sipariþ oluþturulamadý.");
        }
        var dto = await resp.Content.ReadFromJsonAsync<NewOrderResponseDto>();
        return Result.Success(dto!);
    }

    public async Task<Result<MyOrdersResponseDto>> GetMyOrdersAsync(string jwt)
    {
        var resp = await SendAsync("api/order/my", HttpMethod.Get, jwt);
        if (!resp.IsSuccessStatusCode)
        {
            if (resp.StatusCode == System.Net.HttpStatusCode.Unauthorized) return Result.Unauthorized();
            return Result.Error("Sipariþler alýnamadý.");
        }
        var anonList = await resp.Content.ReadFromJsonAsync<List<MyOrderSummaryDto>>();
        return Result.Success(new MyOrdersResponseDto { Orders = anonList ?? new() });
    }

    public async Task<Result<MyOrderDetailResponseDto>> GetOrderDetailsByCodeAsync(string jwt, string orderCode)
    {
        var resp = await SendAsync($"api/order/{orderCode}/details", HttpMethod.Get, jwt);
        if (!resp.IsSuccessStatusCode)
        {
            if (resp.StatusCode == System.Net.HttpStatusCode.Unauthorized) return Result.Unauthorized();
            if (resp.StatusCode == System.Net.HttpStatusCode.NotFound) return Result.NotFound();
            return Result.Error("Sipariþ detayý alýnamadý.");
        }
        var detail = await resp.Content.ReadFromJsonAsync<MyOrderDetailResponseDto>();
        return Result.Success(detail!);
    }
}
