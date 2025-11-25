using App.Business.DTOs;
using App.Data.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace App.DataApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly DbContext _dbContext;

        public UserController(DbContext dbContext)
        {
            _dbContext = dbContext;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            // Db'den kullanıcı listesini sorgula

            var users = await _dbContext.Set<UserEntity>().ToListAsync();

            // gelen Entity'leri Dto'ya çevir.

            var userDTOs = users.Select(u => new UserDTO
            {
                Id = u.Id,
                Name = u.Name
            });

            // json olarak reponse dön

            return Ok(userDTOs);
        }

    }
}
