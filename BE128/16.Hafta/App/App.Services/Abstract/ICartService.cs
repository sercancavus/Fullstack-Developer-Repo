using App.Models.DTO.Cart;
using Ardalis.Result;

namespace App.Services.Abstract
{
    public interface ICartService
    {
        Task<Result<CartGetResult>> GetAsync(int userId);
        Task<Result<CartUpdateResult>> UpdateAsync(CartUpdateRequest cartRequest);
    }
}
