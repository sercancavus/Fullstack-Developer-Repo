using App.Data.Entities;
using App.Data.Infrastructure;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace App.Api.Data.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CategoryController(DataRepository<CategoryEntity> categoryRepo) : ControllerBase
{
    // Admin can create/edit/delete; everyone can list

    [HttpGet]
    [AllowAnonymous]
    public async Task<IActionResult> List()
    {
        var categories = await categoryRepo.GetAll()
            .Select(c => new { c.Id, c.Name, c.Color, c.IconCssClass })
            .ToListAsync();
        return Ok(categories);
    }

    [HttpPost]
    [Authorize(Roles = "admin")]
    public async Task<IActionResult> Create([FromBody] CategoryEntity category)
    {
        category.Id = 0; // ensure add
        var created = await categoryRepo.AddAsync(category);
        return CreatedAtAction(nameof(GetById), new { id = created.Id }, new { created.Id });
    }

    [HttpGet("{id:int}")]
    [AllowAnonymous]
    public async Task<IActionResult> GetById([FromRoute] int id)
    {
        var category = await categoryRepo.GetAll()
            .Where(c => c.Id == id)
            .Select(c => new { c.Id, c.Name, c.Color, c.IconCssClass })
            .FirstOrDefaultAsync();
        if (category is null) return NotFound();
        return Ok(category);
    }

    [HttpPut("{id:int}")]
    [Authorize(Roles = "admin")]
    public async Task<IActionResult> Update([FromRoute] int id, [FromBody] CategoryEntity model)
    {
        var existing = await categoryRepo.GetByIdAsync(id);
        if (existing is null) return NotFound();
        existing.Name = model.Name;
        existing.Color = model.Color;
        existing.IconCssClass = model.IconCssClass;
        await categoryRepo.UpdateAsync(existing);
        return NoContent();
    }

    [HttpDelete("{id:int}")]
    [Authorize(Roles = "admin")]
    public async Task<IActionResult> Delete([FromRoute] int id)
    {
        await categoryRepo.DeleteAsync(id);
        return NoContent();
    }
}
