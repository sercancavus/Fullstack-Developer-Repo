using Ardalis.Result;
using App.Models.DTO;

namespace App.Services.Abstract;

public interface IProfileService
{
    Task<Result<ProfileDetailsDto>> GetProfileAsync(string jwt);
    Task<Result> UpdateProfileAsync(string jwt, UpdateProfileRequestDto updateRequest);
}
