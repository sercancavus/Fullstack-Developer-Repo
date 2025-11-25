using App.Business.DTOs;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace App.Business.Services
{
    public class UserService
    {
        //private readonly DbContext _dbContext;

        //public UserService(DbContext dbContext)
        //{
        //    _dbContext = dbContext;
        //}

        public async Task<List<UserDTO>> GetUsers()
        {
            // DB'den UserEntity'leri çek.
            // Select metodu ile bu verileri UserDTO'ya çevirip döndür

            //return await _dbContext.Set<UserEntity>()
            //    .Select(x => new UserDTO
            //    {
            //        Id  = x.Id,
            //        Name = x.Name
            //    })
            //    .ToListAsync();


            var httpClient = new HttpClient();

            var response = await httpClient.GetAsync("https://localhost:7243/api/user");

            if (!response.IsSuccessStatusCode)
            {
                throw new InvalidOperationException();
            }

            var responseObjects = await response.Content.ReadFromJsonAsync<List<UserDTO>>();

            return responseObjects;

        }
    }
}
