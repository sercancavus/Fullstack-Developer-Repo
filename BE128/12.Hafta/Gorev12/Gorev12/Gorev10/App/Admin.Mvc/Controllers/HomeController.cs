using Microsoft.AspNetCore.Mvc;

namespace Admin.Mvc.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class HomeController : ControllerBase
    {
        [HttpGet]
        public IActionResult Index() => Ok(new { message = "Admin home placeholder" });
    }
}