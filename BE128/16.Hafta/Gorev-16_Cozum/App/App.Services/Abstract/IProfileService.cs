using App.Models.DTO.Profile;
using Ardalis.Result;

namespace App.Services.Abstract
{
    public interface IProfileService
    {
        Task<Result<ProfileGetResult>> GetAsync(int userId);
        Task<Result<ProfileUpdateResult>> UpdateAsync(ProfileUpdateRequest profileRequest);
    }
}
