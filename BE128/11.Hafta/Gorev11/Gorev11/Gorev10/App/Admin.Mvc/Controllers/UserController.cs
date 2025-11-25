using Microsoft.AspNetCore.Mvc;

namespace Admin.Mvc.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        [HttpGet]
        public IActionResult List() => Ok(new { message = "Users list placeholder" });

        [HttpPost("approve/{id}")]
        public IActionResult Approve(int id) => Ok(new { message = "User approve placeholder", id });
    }
}