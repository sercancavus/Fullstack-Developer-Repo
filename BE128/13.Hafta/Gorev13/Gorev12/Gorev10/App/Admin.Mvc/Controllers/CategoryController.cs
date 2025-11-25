using App.Data;
using App.Data.Entities;
using Admin.Mvc.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Admin.Mvc.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CategoryController : ControllerBase
    {
        private readonly AppDbContext _db;
        public CategoryController(AppDbContext db) => _db = db;

        // POST: api/category
        [HttpPost]
        public async Task<IActionResult> Create(CategoryCreateViewModel model)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            var entity = new Category
            {
                Name = model.Name,
                Color = model.Color,
                IconCssClass = model.IconCssClass,
                CreatedAt = DateTime.UtcNow
            };
            _db.Categories.Add(entity);
            await _db.SaveChangesAsync();
            return Ok(new { message = "Kategori oluþturuldu", id = entity.Id });
        }

        // PUT: api/category/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> Edit(int id, CategoryEditViewModel model)
        {
            if (id != model.Id) return BadRequest("Id uyuþmuyor");
            if (!ModelState.IsValid) return BadRequest(ModelState);
            var c = await _db.Categories.FirstOrDefaultAsync(x => x.Id == id);
            if (c == null) return NotFound();
            c.Name = model.Name;
            c.Color = model.Color;
            c.IconCssClass = model.IconCssClass;
            await _db.SaveChangesAsync();
            return Ok(new { message = "Kategori güncellendi", id = c.Id });
        }

        // DELETE: api/category/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var c = await _db.Categories.FirstOrDefaultAsync(x => x.Id == id);
            if (c == null) return NotFound();
            _db.Categories.Remove(c);
            await _db.SaveChangesAsync();
            return Ok(new { message = "Kategori silindi", id });
        }
    }
}