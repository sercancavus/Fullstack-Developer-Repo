using App.Api.Data.Entities;
using App.Api.Data.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace App.Api.Controllers
{
    public class ProductCommentController : Controller
    {
        private readonly DataRepository<ProductComment> _repo;

        public ProductCommentController(DataRepository<ProductComment> repo)
        {
            _repo = repo;
        }

        // Only users who purchased can comment - Buyer,Seller
        [Authorize(Roles = "Buyer,Seller")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ProductComment model)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            await _repo.AddAsync(model);
            return RedirectToAction("Details", "Product", new { id = model.ProductId });
        }

        // Admin can approve comments
        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> Approve(int id)
        {
            var c = await _repo.GetByIdAsync(id);
            if (c == null) return NotFound();
            c.IsApproved = true;
            await _repo.UpdateAsync(c);
            return RedirectToAction("Index", "Product");
        }
    }
}
