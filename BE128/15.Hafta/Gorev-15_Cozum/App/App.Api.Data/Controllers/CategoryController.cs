using App.Data.Entities;
using App.Data.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace App.Api.Data.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController(IDataRepository repo) : ControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var categories = await repo.GetAll<CategoryEntity>().ToListAsync();
            return Ok(categories);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var category = await repo.GetByIdAsync<CategoryEntity>(id);
            if (category is null)
            {
                return NotFound();
            }

            return Ok(category);
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] CategoryEntity category)
        {
            await repo.AddAsync(category);
            return CreatedAtAction(nameof(Get), new { id = category.Id }, category);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, [FromBody] CategoryEntity category)
        {
            if (id != category.Id)
            {
                return BadRequest();
            }

            await repo.UpdateAsync(category);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            if (!await repo.GetAll<CategoryEntity>().AnyAsync(c => c.Id == id))
            {
                return NotFound();
            }

            await repo.DeleteAsync<CategoryEntity>(id);
            return NoContent();
        }
    }
}
