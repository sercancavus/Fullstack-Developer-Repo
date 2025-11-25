using Ardalis.Result;
using App.Models.DTO;
using App.Services.Abstract;
using System.Net.Http;
using System.Net.Http.Json;

namespace App.Services.Concrete;

public class ProductCommentService : IProductCommentService
{
    private readonly HttpClient client;
    public ProductCommentService(IHttpClientFactory httpClientFactory)
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

    public async Task<Result<List<ProductCommentDto>>> ListPendingCommentsAsync(string jwt)
    {
        var response = await SendApiRequestAsync("api/admin/comments/pending", HttpMethod.Get, jwt);
        if (!response.IsSuccessStatusCode)
        {
            if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                return Result.Unauthorized();
            return Result.Error("Yorumlar alýnamadý.");
        }
        var comments = await response.Content.ReadFromJsonAsync<List<ProductCommentDto>>();
        return Result.Success(comments ?? new List<ProductCommentDto>());
    }

    public async Task<Result> ApproveCommentAsync(string jwt, int commentId)
    {
        var response = await SendApiRequestAsync($"api/admin/comments/{commentId}/approve", HttpMethod.Post, jwt);
        if (!response.IsSuccessStatusCode)
        {
            if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                return Result.Unauthorized();
            if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
                return Result.NotFound();
            return Result.Error("Yorum onaylanamadý.");
        }
        return Result.Success();
    }
}
