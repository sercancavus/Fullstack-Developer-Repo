using App.Business.DTOs;
using App.Data.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Business.Services
{
    public class UserService
    {
        private readonly DbContext _dbContext;

        public UserService(DbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<List<UserDTO>> GetUsers()
        {
            // DB'den UserEntity'leri çek.
            // Select metodu ile bu verileri UserDTO'ya çevirip döndür

            return await _dbContext.Set<UserEntity>()
                .Select(x => new UserDTO
                {
                    Id  = x.Id,
                    Name = x.Name
                })
                .ToListAsync();

        }
    }
}
