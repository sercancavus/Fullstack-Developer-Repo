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
        private readonly IHttpClientFactory _clientFactory;

        private HttpClient Client => _clientFactory.CreateClient("data-api");

        public UserService(IHttpClientFactory clientFactory)
        {
            _clientFactory = clientFactory;
        }

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


            //var httpClient = new HttpClient();

            //var response = await httpClient.GetAsync("https://localhost:7243/api/user");



            var response = await Client.GetAsync("/api/user");

            if (!response.IsSuccessStatusCode)
            {
                throw new InvalidOperationException();
            }

            var responseObjects = await response.Content.ReadFromJsonAsync<List<UserDTO>>();

            return responseObjects;

        }
    }
}
