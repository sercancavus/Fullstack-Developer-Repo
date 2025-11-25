using App.Api.Data.Models;
using App.Data.Entities;
using App.Data.Infrastructure;
using App.Models.DTO.User;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace App.Api.Data.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class UserController(IDataRepository dataRepository) : ControllerBase
    {

        [HttpPost("login", Name = "GetUser")]
        public async Task<IActionResult> Get([FromBody] LoginDto login)
        {

            if (string.IsNullOrEmpty(login.Email) || string.IsNullOrEmpty(login.Password))
            {
                return BadRequest();
            }

            var user = await dataRepository.GetAll<UserEntity>()
                .Where(u => u.Enabled && u.Email == login.Email && u.Password == login.Password)
                .Select(u => new UserGetResult
                {
                    Id = u.Id,
                    Email = u.Email,
                    FirstName = u.FirstName,
                    LastName = u.LastName,
                    Role = u.Role.Name,
                    Enabled = u.Enabled,
                    HasSellerRequest = u.HasSellerRequest
                })
                .SingleOrDefaultAsync();

            if (user is null)
            {
                return NotFound();
            }

            return Ok(user);
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var users = await dataRepository.GetAll<UserEntity>()
                .Select(u => new UserGetResult
                {
                    Id = u.Id,
                    Email = u.Email,
                    FirstName = u.FirstName,
                    LastName = u.LastName,
                    Role = u.Role.Name,
                    Enabled = u.Enabled,
                    HasSellerRequest = u.HasSellerRequest
                })
                .ToListAsync();

            return Ok(users);
        }


        [HttpGet("reset-password-token/{token}")]
        public async Task<IActionResult> GetUserByResetToken(string token)
        {
            var user = await dataRepository.GetAll<UserEntity>()
                .FirstOrDefaultAsync(u => u.ResetPasswordToken == token);

            if (user is null)
            {
                return NotFound();
            }

            user.Password = string.Empty;
            user.ResetPasswordToken = string.Empty;

            return Ok(user);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var user = await dataRepository.GetAll<UserEntity>()
                .Where(u => u.Id == id)
                .Select(u => new UserGetResult
                {
                    Id = u.Id,
                    Email = u.Email,
                    FirstName = u.FirstName,
                    LastName = u.LastName,
                    Role = u.Role.Name,
                    Enabled = u.Enabled,
                    HasSellerRequest = u.HasSellerRequest
                })
                .SingleOrDefaultAsync();

            if (user is null)
            {
                return NotFound();
            }

            return Ok(user);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, [FromBody] UserEntity user)
        {
            if (id != user.Id)
            {
                return BadRequest();
            }

            await dataRepository.UpdateAsync(user);
            return NoContent();
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] UserEntity user)
        {
            user = await dataRepository.AddAsync(user);
            return CreatedAtRoute("GetUser", new { id = user.Id }, user);
        }
    }
}
