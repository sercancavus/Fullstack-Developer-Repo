using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using App.Data.Entities;
using App.Data.Infrastructure;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace App.Api.Data.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController(DataRepository<UserEntity> userRepo, IConfiguration configuration) : ControllerBase
{
    public class LoginRequest
    {
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string App { get; set; } = ""; // "admin" or "e-ticaret"
    }

    public record LoginResponse(string Token);

    public class RegisterRequest
    {
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
    }

    [HttpPost("register")]
    [AllowAnonymous]
    public async Task<IActionResult> Register([FromBody] RegisterRequest request)
    {
        if (string.IsNullOrWhiteSpace(request.Email) || string.IsNullOrWhiteSpace(request.Password) ||
            string.IsNullOrWhiteSpace(request.FirstName) || string.IsNullOrWhiteSpace(request.LastName))
        {
            return BadRequest();
        }

        var exists = await userRepo.GetAll().AnyAsync(u => u.Email == request.Email);
        if (exists)
        {
            return Conflict();
        }

        var user = new UserEntity
        {
            FirstName = request.FirstName,
            LastName = request.LastName,
            Email = request.Email,
            Password = request.Password,
            RoleId = 3, // buyer
            Enabled = true
        };

        user = await userRepo.AddAsync(user);
        return Created($"/api/profile/{user.Id}", new { user.Id });
    }

    [HttpPost("login")]
    [AllowAnonymous]
    public async Task<IActionResult> Login([FromBody] LoginRequest request)
    {
        var user = await userRepo.GetAll()
            .Include(u => u.Role)
            .FirstOrDefaultAsync(u => u.Email == request.Email && u.Password == request.Password);

        if (user is null)
        {
            return NotFound();
        }

        // Cross-app login restriction
        var role = user.Role.Name.ToLowerInvariant();
        var appName = (request.App ?? string.Empty).ToLowerInvariant();
        if (appName == "admin" && role != "admin")
        {
            return Unauthorized();
        }
        if (appName == "eticaret" && role == "admin")
        {
            return Unauthorized();
        }

        var claims = new List<Claim>
        {
            new(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new(ClaimTypes.Name, user.FirstName),
            new(ClaimTypes.Surname, user.LastName),
            new(ClaimTypes.Email, user.Email),
            new(ClaimTypes.Role, role)
        };

        var jwtSection = configuration.GetSection("Jwt");
        var issuer = jwtSection["Issuer"] ?? "AppIssuer";
        var audience = jwtSection["Audience"] ?? "AppAudience";
        var secret = jwtSection["Secret"] ?? "SuperSecretKey_MinLen32_ChangeMe!";
        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secret));
        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: issuer,
            audience: audience,
            claims: claims,
            expires: DateTime.UtcNow.AddHours(2),
            signingCredentials: credentials
        );

        var tokenHandler = new JwtSecurityTokenHandler();
        var jwt = tokenHandler.WriteToken(token);

        return Ok(new LoginResponse(jwt));
    }
}
