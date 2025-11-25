using Ardalis.Result;
using App.Models.DTO;

namespace App.Services.Abstract;

public interface IUserService
{
    Task<Result<List<UserDto>>> ListUsersAsync(string jwt);
    Task<Result> EnableUserAsync(string jwt, int userId);
    Task<Result> DisableUserAsync(string jwt, int userId);
    Task<Result> ApproveSellerRequestAsync(string jwt, int userId);
}
