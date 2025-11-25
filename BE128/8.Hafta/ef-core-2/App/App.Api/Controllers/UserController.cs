using App.Api.Data;
using App.Api.Data.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace App.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        //private static List<UserModel> Users = new();

        //private AppDbContext Context { get; }

        //public UserController()
        //{
        //    Context = new AppDbContext();

        //    // EnsureCreated metodu ilgili isimde veritabanı yok ise oluşturur
        //    // var ise hiç bir şey yapmaz.

        //    Context.Database.EnsureCreated(); 
        //}

        private AppDbContext Context { get; }

        public UserController(AppDbContext dbContext)
        {
            Context = dbContext;
        }


        [HttpGet]
        public IActionResult Get()
        {
            //return Ok(Users);

            // ----------------------------------

            var users = Context.Users.ToList(); // Veritabanındaki select işlemi

            // Context : Veritabanı

            // Users : Veritabanı içindeki Users tablosu

            return Ok(users);

        }

        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            //var user = Users.Find(x => x.Id == id);

            //if (user == null)
            //{
            //    return NotFound();
            //}

            //return Ok(user);

            // ----------------------------------

            var user = Context.Users.FirstOrDefault(x => x.Id == id);

            if (user == null)
            {
                return NotFound();
            }

            return Ok(user);

        }

        [HttpPost]
        public IActionResult Create([FromBody] UserEntity user)
        {
            //if (!ModelState.IsValid)
            //{
            //    return BadRequest(ModelState);
            //}

            //user.Id = Users.Count + 1;
            //Users.Add(user);

            //return Ok(user);

            // ----------------------------------

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            user.Id = 0;

            Context.Users.Add(user);  // INSERT INTO

            // Gerçekten DB'ına ekleme yapması için değişkilikleri kaydet dememiz lazım.
            Context.SaveChanges();

            return Ok(user);
        }

        [HttpPut("{id}")]
        public IActionResult Update([FromRoute] int id, [FromBody] UserEntity user)
        {
            //if (!ModelState.IsValid)
            //{
            //    return BadRequest(ModelState);
            //}

            //var userIndex = Users.FindIndex(x => x.Id == id);

            //if (userIndex < 0)
            //{
            //    return NotFound();
            //}

            //user.Id = id;

            //Users[userIndex] = user;

            //return Ok(user);


            // ----------------------------------


            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var dbUser = Context.Users.FirstOrDefault(u => u.Id == id);

            if (dbUser is null)
            {
                return NotFound();
            }

            dbUser.Name = user.Name;
            dbUser.Email = user.Email;
            dbUser.Password = user.Password;

            Context.SaveChanges();

            return Ok(user);
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            //var userIndex = Users.FindIndex(x => x.Id == id);

            //if (userIndex < 0)
            //{
            //    return NotFound();
            //}

            //Users.RemoveAt(userIndex);

            //return Ok();

            // ----------------------------------

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var dbUser = Context.Users.FirstOrDefault(u => u.Id == id);

            if (dbUser is null)
            {
                return NotFound();
            }

            Context.Users.Remove(dbUser);

            Context.SaveChanges();

            return Ok();
        }
    }
}
