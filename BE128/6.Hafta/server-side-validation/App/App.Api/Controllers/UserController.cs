using App.Api.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace App.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private static List<UserModel> Users = new();

        [HttpPost("register")]
        public IActionResult Register([FromBody] UserModel model)
        {
            // Modelin durumu geçerli mi? -> Modelin durumu geçerli değilse, BadRequest dön!
            // Is model state valid?  -> If model state is not valid, return BadRequest!

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            Users.Add(model);
            return Ok();
        }

    }
}
