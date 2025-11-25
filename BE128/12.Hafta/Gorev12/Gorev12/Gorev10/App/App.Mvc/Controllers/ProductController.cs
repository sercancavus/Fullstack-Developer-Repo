using App.Data;
using App.Data.Entities;
using App.Mvc.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace App.Mvc.Controllers
{
    public class ProductController : Controller
    {
        private readonly AppDbContext _db;
        public ProductController(AppDbContext db) => _db = db;

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            ViewBag.Categories = await _db.Categories.OrderBy(c => c.Name).ToListAsync();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ProductCreateViewModel model)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Categories = await _db.Categories.OrderBy(c => c.Name).ToListAsync();
                return View(model);
            }

            var entity = new Product
            {
                Name = model.Name,
                CategoryId = model.CategoryId,
                Price = model.Price,
                Details = model.Details ?? string.Empty,
                StockAmount = model.StockAmount,
                CreatedAt = DateTime.UtcNow,
                Enabled = true,
                SellerId = 1 // TODO: auth sonrasý aktif kullanýcý id
            };
            _db.Products.Add(entity);
            await _db.SaveChangesAsync();

            TempData["Success"] = "Ürün baþarýyla oluþturuldu.";
            return RedirectToAction(nameof(Edit), new { id = entity.Id });
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var p = await _db.Products.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);
            if (p == null) return NotFound();

            var vm = new ProductEditViewModel
            {
                Id = p.Id,
                Name = p.Name,
                CategoryId = p.CategoryId,
                Price = p.Price,
                Details = p.Details,
                StockAmount = p.StockAmount,
                Enabled = p.Enabled
            };
            ViewBag.Categories = await _db.Categories.OrderBy(c => c.Name).ToListAsync();
            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(ProductEditViewModel model)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Categories = await _db.Categories.OrderBy(c => c.Name).ToListAsync();
                return View(model);
            }

            var p = await _db.Products.FirstOrDefaultAsync(x => x.Id == model.Id);
            if (p == null) return NotFound();

            p.Name = model.Name;
            p.CategoryId = model.CategoryId;
            p.Price = model.Price;
            p.Details = model.Details ?? string.Empty;
            p.StockAmount = model.StockAmount;
            p.Enabled = model.Enabled;

            await _db.SaveChangesAsync();
            ViewBag.Categories = await _db.Categories.OrderBy(c => c.Name).ToListAsync();
            ViewBag.Message = "Ürün güncellendi.";
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var p = await _db.Products.FirstOrDefaultAsync(x => x.Id == id);
            if (p == null)
            {
                TempData["Error"] = "Silinecek ürün bulunamadý.";
                return RedirectToAction("Index", "Home");
            }

            _db.Products.Remove(p);
            await _db.SaveChangesAsync();
            TempData["Success"] = "Ürün silindi.";
            return RedirectToAction("Listing", "Home");
        }

        [HttpGet]
        public IActionResult Comment(int productId)
        {
            var vm = new ProductCommentViewModel { ProductId = productId };
            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Comment(ProductCommentViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var productExists = await _db.Products.AnyAsync(p => p.Id == model.ProductId);
            if (!productExists)
            {
                ModelState.AddModelError(string.Empty, "Ürün bulunamadý");
                return View(model);
            }
            var entity = new ProductComment
            {
                ProductId = model.ProductId,
                UserId = 1, // TODO: auth sonrasý gerçek kullanýcý
                Text = model.Text,
                StarCount = model.StarCount,
                IsConfirmed = false,
                CreatedAt = DateTime.UtcNow
            };
            _db.ProductComments.Add(entity);
            await _db.SaveChangesAsync();
            TempData["Success"] = "Yorumunuz alýndý, onay sonrasý yayýnlanacaktýr.";
            return RedirectToAction("ProductDetail", "Home", new { id = model.ProductId, categoryName = "", title = "" });
        }
    }
}