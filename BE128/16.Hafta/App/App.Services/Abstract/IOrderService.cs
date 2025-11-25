using App.Models.DTO.Order;
using Ardalis.Result;

namespace App.Services.Abstract
{
    public interface IOrderService
    {
        Task<Result<List<OrderGetResult>>> GetAsync(OrderGetRequest getRequest);
        Task<Result<OrderPlaceResult>> PlaceAsync(OrderPlaceRequest orderRequest);
    }
}
