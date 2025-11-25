using App.Api.Data.Entities;
using App.Api.Data.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace App.Api.Controllers
{
    [Authorize]
    public class ProfileController : Controller
    {
        private readonly UserManager<User> _userManager;
        private readonly DataRepository<User> _userRepo;

        public ProfileController(UserManager<User> userManager, DataRepository<User> userRepo)
        {
            _userManager = userManager;
            _userRepo = userRepo;
        }

        [Authorize(Roles = "Buyer,Seller,Admin")]
        public async Task<IActionResult> Index()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null) return RedirectToAction("Login", "Account");
            return View(user);
        }

        [Authorize(Roles = "Buyer")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RequestSeller()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null) return RedirectToAction("Login", "Account");
            user.IsSellerRequested = true;
            await _userRepo.UpdateAsync(user);
            return RedirectToAction("Index");
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ToggleActive(string userId)
        {
            var u = await _userRepo.GetByIdAsync(userId);
            if (u == null) return NotFound();
            u.IsActive = !u.IsActive;
            await _userRepo.UpdateAsync(u);
            return RedirectToAction("Index", "UserManagement");
        }
    }
}
