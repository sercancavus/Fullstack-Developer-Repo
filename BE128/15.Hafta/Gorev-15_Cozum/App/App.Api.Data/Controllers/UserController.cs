using App.Api.Data.Models;
using App.Data.Entities;
using App.Data.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace App.Api.Data.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
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
                .Include(u => u.Role)
                .SingleOrDefaultAsync(u => u.Email == login.Email && u.Password == login.Password);

            if (user is null)
            {
                return NotFound();
            }

            user.Password = string.Empty;
            user.ResetPasswordToken = string.Empty;

            return Ok(user);
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var users = await dataRepository.GetAll<UserEntity>()
                .Include(u => u.Role)
                .ToListAsync();

            foreach (var user in users)
            {
                user.Password = string.Empty;
                user.ResetPasswordToken = string.Empty;
            }

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
            var user = await dataRepository.GetByIdAsync<UserEntity>(id);

            if (user is null)
            {
                return NotFound();
            }

            user.Password = string.Empty;
            user.ResetPasswordToken = string.Empty;

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
