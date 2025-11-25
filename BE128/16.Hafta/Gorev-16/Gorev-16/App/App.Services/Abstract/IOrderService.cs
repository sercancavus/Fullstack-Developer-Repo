using Ardalis.Result;
using App.Models.DTO;

namespace App.Services.Abstract;

public interface IOrderService
{
    Task<Result<NewOrderResponseDto>> PlaceOrderAsync(string jwt, PlaceOrderRequestDto request);
    Task<Result<MyOrdersResponseDto>> GetMyOrdersAsync(string jwt);
    Task<Result<MyOrderDetailResponseDto>> GetOrderDetailsByCodeAsync(string jwt, string orderCode);
}
