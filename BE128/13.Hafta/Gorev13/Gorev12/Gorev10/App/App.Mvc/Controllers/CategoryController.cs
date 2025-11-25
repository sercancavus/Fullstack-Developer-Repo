using App.Data;
using App.Data.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace App.Mvc.Controllers
{
    [Authorize(Roles = "admin")]
    public class CategoryController : Controller
    {
        private readonly AppDbContext _db;
        public CategoryController(AppDbContext db) => _db = db;

        // Tüm kategorileri listele
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var categories = await _db.Categories.Include(c => c.Products).ToListAsync();
            return View(categories);
        }

        [HttpGet]
        public IActionResult Create() => View();

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Category model)
        {
            if (!ModelState.IsValid)
                return View(model);
            model.CreatedAt = DateTime.UtcNow;
            _db.Categories.Add(model);
            await _db.SaveChangesAsync();
            TempData["Success"] = "Kategori baþarýyla eklendi.";
            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var category = await _db.Categories.Include(c => c.Products).FirstOrDefaultAsync(c => c.Id == id);
            if (category == null) return NotFound();
            return View(category);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Category model)
        {
            if (!ModelState.IsValid)
                return View(model);
            var category = await _db.Categories.FirstOrDefaultAsync(c => c.Id == model.Id);
            if (category == null) return NotFound();
            category.Name = model.Name;
            category.Color = model.Color;
            category.IconCssClass = model.IconCssClass;
            await _db.SaveChangesAsync();
            TempData["Success"] = "Kategori baþarýyla güncellendi.";
            return RedirectToAction("Index");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var category = await _db.Categories.FirstOrDefaultAsync(c => c.Id == id);
            if (category == null) return NotFound();
            _db.Categories.Remove(category);
            await _db.SaveChangesAsync();
            TempData["Success"] = "Kategori silindi.";
            return RedirectToAction("Index");
        }
    }
}
