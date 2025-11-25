using Ardalis.Result;
using App.Models.DTO;

namespace App.Services.Abstract;

public interface ICartService
{
    Task<Result<List<CartItemDto>>> GetMyCartAsync(string jwt);
    Task<Result<CartSummaryDto>> GetCartSummaryAsync(string jwt);
    Task<Result> AddToCartAsync(string jwt, int productId);
    Task<Result> RemoveFromCartAsync(string jwt, int cartItemId);
    Task<Result<CartItemDto>> UpdateCartItemAsync(string jwt, UpdateCartItemRequestDto updateRequest);
}
