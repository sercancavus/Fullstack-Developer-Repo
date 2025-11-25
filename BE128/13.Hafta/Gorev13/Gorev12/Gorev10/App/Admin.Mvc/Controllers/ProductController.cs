using Microsoft.AspNetCore.Mvc;

namespace Admin.Mvc.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductController : ControllerBase
    {
        [HttpDelete("{id}")]
        public IActionResult Delete(int id) => Ok(new { message = "Product delete placeholder", id });
    }
}