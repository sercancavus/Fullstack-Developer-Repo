using App.Api.Data.Entities;
using App.Api.Data.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace App.Api.Controllers
{
    public class OrderController : Controller
    {
        private readonly DataRepository<Order> _repo;

        public OrderController(DataRepository<Order> repo)
        {
            _repo = repo;
        }

        // Create order after payment - Buyer and Seller
        [Authorize(Roles = "Buyer,Seller")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Order model)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            await _repo.AddAsync(model);
            return RedirectToAction("Details", new { id = model.Id });
        }

        [Authorize(Roles = "Buyer,Seller")]
        public async Task<IActionResult> List(string userId)
        {
            var all = await _repo.GetAllAsync();
            var my = all.Where(x => x.UserId == userId).ToList();
            return View(my);
        }

        [Authorize(Roles = "Buyer,Seller")]
        public async Task<IActionResult> Details(int id)
        {
            var o = await _repo.GetByIdAsync(id);
            if (o == null) return NotFound();
            return View(o);
        }
    }
}
