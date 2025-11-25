using Ardalis.Result;
using App.Models.DTO;

namespace App.Services.Abstract;

public interface IProductService
{
    Task<Result<List<ProductDto>>> ListProductsAsync(string jwt);
    Task<Result<ProductDetailDto>> GetProductDetailAsync(string jwt, int productId);
    Task<Result<CreateProductResponseDto>> CreateProductAsync(string jwt, CreateProductRequestDto request);
    // Diðer metotlar: Update, Delete, Comment, etc.
}
