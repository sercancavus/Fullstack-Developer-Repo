using Microsoft.AspNetCore.Mvc;

namespace Admin.Mvc.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CommentController : ControllerBase
    {
        [HttpGet]
        public IActionResult List() => Ok(new { message = "Comments list placeholder" });

        [HttpPost("approve/{id}")]
        public IActionResult Approve(int id) => Ok(new { message = "Comment approved placeholder", id });
    }
}