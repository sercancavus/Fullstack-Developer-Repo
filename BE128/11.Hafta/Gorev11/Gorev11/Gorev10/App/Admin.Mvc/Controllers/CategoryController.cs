using Microsoft.AspNetCore.Mvc;

namespace Admin.Mvc.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CategoryController : ControllerBase
    {
        // POST: api/category
        [HttpPost]
        public IActionResult Create() => Ok(new { message = "Category create placeholder" });

        // PUT: api/category/{id}
        [HttpPut("{id}")]
        public IActionResult Edit(int id) => Ok(new { message = "Category edit placeholder", id });

        // DELETE: api/category/{id}
        [HttpDelete("{id}")]
        public IActionResult Delete(int id) => Ok(new { message = "Category delete placeholder", id });
    }
}