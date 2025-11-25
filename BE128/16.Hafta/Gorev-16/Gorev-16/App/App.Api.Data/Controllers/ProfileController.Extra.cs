using App.Data.Entities;
using App.Data.Infrastructure;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace App.Api.Data.Controllers;

public partial class ProfileController
{
    [HttpPut]
    public async Task<IActionResult> UpdateMe([FromBody] UpdateProfileRequest req)
    {
        var userIdStr = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (!int.TryParse(userIdStr, out var userId)) return Unauthorized();

        var user = await userRepo.GetByIdAsync(userId);
        if (user is null) return NotFound();

        user.FirstName = req.FirstName;
        user.LastName = req.LastName;
        if (!string.IsNullOrWhiteSpace(req.Password))
        {
            user.Password = req.Password;
        }

        await userRepo.UpdateAsync(user);
        return NoContent();
    }

    public record UpdateProfileRequest(string FirstName, string LastName, string? Password);
}
