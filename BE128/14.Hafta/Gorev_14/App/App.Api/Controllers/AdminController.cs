using App.Api.Data.Entities;
using App.Api.Data.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace App.Api.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public AdminController(UserManager<User> userManager, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public async Task<IActionResult> Users()
        {
            var users = await _userManager.Users.ToListAsync();
            return View(users);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ToggleActive(string userId)
        {
            var u = await _userManager.FindByIdAsync(userId);
            if (u == null) return NotFound();
            u.IsActive = !u.IsActive;
            await _userManager.UpdateAsync(u);
            return RedirectToAction("Users");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ApproveSeller(string userId)
        {
            var u = await _userManager.FindByIdAsync(userId);
            if (u == null) return NotFound();
            // Ensure role exists
            if (!await _roleManager.RoleExistsAsync("Seller"))
            {
                await _roleManager.CreateAsync(new IdentityRole("Seller"));
            }
            // Add role
            if (!await _userManager.IsInRoleAsync(u, "Seller"))
            {
                await _userManager.AddToRoleAsync(u, "Seller");
            }
            // mark request handled
            u.IsSellerRequested = false;
            await _userManager.UpdateAsync(u);
            return RedirectToAction("Users");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SetRole(string userId, string role)
        {
            var u = await _userManager.FindByIdAsync(userId);
            if (u == null) return NotFound();
            if (!await _roleManager.RoleExistsAsync(role))
            {
                await _roleManager.CreateAsync(new IdentityRole(role));
            }
            var currentRoles = await _userManager.GetRolesAsync(u);
            await _userManager.RemoveFromRolesAsync(u, currentRoles);
            await _userManager.AddToRoleAsync(u, role);
            return RedirectToAction("Users");
        }
    }
}
