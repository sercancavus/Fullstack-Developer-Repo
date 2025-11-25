using App.Data.Entities;
using App.Data.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace App.Api.Controllers
{
    // Seller can create, list and update own products; Buyers can view and search; Admin can deactivate
    public class ProductController : Controller
    {
        private readonly DataRepository<Product> _repo;

        public ProductController(DataRepository<Product> repo)
        {
            _repo = repo;
        }

        [Authorize(Roles = "Seller,Admin")]
        public async Task<IActionResult> Create()
        {
            return View(new Product());
        }

        [HttpPost]
        [Authorize(Roles = "Seller,Admin")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Product model)
        {
            if (!ModelState.IsValid) return View(model);
            await _repo.AddAsync(model);
            return RedirectToAction("MyProducts");
        }

        [Authorize(Roles = "Seller")]
        public async Task<IActionResult> MyProducts()
        {
            // In full implementation filter by seller id
            var list = await _repo.GetAllAsync();
            return View(list);
        }

        // Details allowed for Buyer and Seller per table
        [Authorize(Roles = "Buyer,Seller")]
        public async Task<IActionResult> Details(int id)
        {
            var p = await _repo.GetByIdAsync(id);
            if (p == null) return NotFound();
            return View(p);
        }

        // Search allowed for Buyer and Seller per table
        [Authorize(Roles = "Buyer,Seller")]
        public async Task<IActionResult> Search(string q)
        {
            var all = await _repo.GetAllAsync();
            var filtered = all.Where(x => x.Name.Contains(q ?? string.Empty, System.StringComparison.OrdinalIgnoreCase)).ToList();
            return View(filtered);
        }

        [Authorize(Roles = "Seller,Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdatePrice(int id, decimal price)
        {
            var p = await _repo.GetByIdAsync(id);
            if (p == null) return NotFound();
            p.Price = price;
            await _repo.UpdateAsync(p);
            return RedirectToAction("MyProducts");
        }

        [Authorize(Roles = "Seller,Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateStock(int id, int stock)
        {
            var p = await _repo.GetByIdAsync(id);
            if (p == null) return NotFound();
            p.Stock = stock;
            await _repo.UpdateAsync(p);
            return RedirectToAction("MyProducts");
        }

        [Authorize(Roles = "Seller,Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ToggleActive(int id)
        {
            var p = await _repo.GetByIdAsync(id);
            if (p == null) return NotFound();
            p.IsActive = !p.IsActive;
            await _repo.UpdateAsync(p);
            return RedirectToAction("MyProducts");
        }
    }
}