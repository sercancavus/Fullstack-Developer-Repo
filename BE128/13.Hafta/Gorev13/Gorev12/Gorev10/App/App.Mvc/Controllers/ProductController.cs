using App.Data;
using App.Data.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace App.Mvc.Controllers
{
    [Authorize(Roles = "admin")]
    public class ProductController : Controller
    {
        private readonly AppDbContext _db;
        public ProductController(AppDbContext db) => _db = db;

        // Tüm ürünleri listele
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var products = await _db.Products.Include(p => p.Category).ToListAsync();
            return View(products);
        }

        [HttpGet]
        public IActionResult Create()
        {
            ViewBag.Categories = _db.Categories.ToList();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Product model)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Categories = _db.Categories.ToList();
                return View(model);
            }
            model.CreatedAt = DateTime.UtcNow;
            model.Enabled = true;
            _db.Products.Add(model);
            await _db.SaveChangesAsync();
            TempData["Success"] = "Ürün baþarýyla eklendi.";
            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var product = await _db.Products.Include(p => p.Category).FirstOrDefaultAsync(p => p.Id == id);
            if (product == null) return NotFound();
            ViewBag.Categories = _db.Categories.ToList();
            return View(product);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Product model)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Categories = _db.Categories.ToList();
                return View(model);
            }
            var product = await _db.Products.FirstOrDefaultAsync(p => p.Id == model.Id);
            if (product == null) return NotFound();
            product.Name = model.Name;
            product.CategoryId = model.CategoryId;
            product.Price = model.Price;
            product.Details = model.Details;
            product.StockAmount = model.StockAmount;
            product.Enabled = model.Enabled;
            await _db.SaveChangesAsync();
            TempData["Success"] = "Ürün baþarýyla güncellendi.";
            return RedirectToAction("Index");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var product = await _db.Products.FirstOrDefaultAsync(p => p.Id == id);
            if (product == null) return NotFound();
            _db.Products.Remove(product);
            await _db.SaveChangesAsync();
            TempData["Success"] = "Ürün silindi.";
            return RedirectToAction("Index");
        }
    }
}