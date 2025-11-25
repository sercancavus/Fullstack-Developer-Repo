using App.Models.DTO.Category;
using Ardalis.Result;

namespace App.Services.Abstract
{
    public interface ICategoryService
    {
        Task<Result<List<CategoryGetResult>>> GetAsync();
        Task<Result<CategoryGetResult>> GetAsync(int categoryId);
        Task<Result<CategoryCreateResult>> CreateAsync(CategoryCreateRequest categoryRequest);
    }
}
