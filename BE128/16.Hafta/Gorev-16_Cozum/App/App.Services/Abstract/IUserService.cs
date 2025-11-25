using App.Models.DTO.User;
using Ardalis.Result;

namespace App.Services.Abstract
{
    public interface IUserService
    {
        Task<Result<UserGetResult>> GetAsync(int userId);
        Task<PagedResult<List<UserGetResult>>> GetPagedAsync(UserGetPagedRequest getRequest);
        Task<Result<UserUpdateResult>> UpdateAsync(UserUpdateRequest userRequest);
        Task<Result<UserChangeRoleResult>> ChangeRoleAsync(UserChangeRoleRequest roleRequest);
        Task<Result<UserChangeStatusResult>> ChangeStatusAsync(UserChangeStatusRequest statusRequest);
    }
}
