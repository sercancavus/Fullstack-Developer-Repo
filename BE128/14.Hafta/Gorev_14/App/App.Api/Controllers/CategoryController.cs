using App.Data.Entities;
using App.Data.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace App.Api.Controllers
{
    public class CategoryController : Controller
    {
        private readonly DataRepository<Category> _repo;

        public CategoryController(DataRepository<Category> repo)
        {
            _repo = repo;
        }

        // Listing categories - allowed for Buyer, Seller per task table
        [Authorize(Roles = "Buyer,Seller")]
        public async Task<IActionResult> Index()
        {
            var list = await _repo.GetAllAsync();
            return View(list);
        }

        // Create - Admin only
        [Authorize(Roles = "Admin")]
        public IActionResult Create()
        {
            return View(new Category());
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Category model)
        {
            if (!ModelState.IsValid) return View(model);
            await _repo.AddAsync(model);
            return RedirectToAction("Index");
        }
    }
}