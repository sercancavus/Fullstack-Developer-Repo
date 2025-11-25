using App.Models.DTO.Product;
using Ardalis.Result;

namespace App.Services.Abstract
{
    public interface IProductService
    {
        Task<Result<List<ProductGetResult>>> GetAsync();
        Task<Result<ProductGetResult>> GetAsync(int productId);
        Task<PagedResult<List<ProductGetResult>>> GetPagedAsync(ProductGetPagedRequest getRequest);
        Task<Result<ProductCreateResult>> CreateAsync(ProductCreateRequest productRequest);
        Task<Result<ProductUpdateResult>> UpdateAsync(ProductUpdateRequest productRequest);
        Task<Result<ProductReviewResult>> ReviewAsync(ProductReviewRequest reviewRequest);
        Task<Result<ProductChangeStatusResult>> ChangeStatusAsync(ProductChangeStatusRequest statusRequest);
        Task<Result<ProductChangeStockResult>> ChangeStockAsync(ProductChangeStockRequest stockRequest);
    }
}
