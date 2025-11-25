using Ardalis.Result;
using App.Models.DTO;

namespace App.Services.Abstract;

public interface ICategoryService
{
    Task<Result<List<CategoryDto>>> ListCategoriesAsync(string jwt);
    Task<Result<CategoryDto>> GetCategoryAsync(string jwt, int categoryId);
    Task<Result<CreateCategoryResponseDto>> CreateCategoryAsync(string jwt, CreateCategoryRequestDto request);
    Task<Result> UpdateCategoryAsync(string jwt, int categoryId, UpdateCategoryRequestDto request);
    Task<Result> DeleteCategoryAsync(string jwt, int categoryId);
}
