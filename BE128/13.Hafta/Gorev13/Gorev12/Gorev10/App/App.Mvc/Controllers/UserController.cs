using App.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using App.Data.Entities;

namespace App.Mvc.Controllers
{
    [Authorize(Roles = "admin")]
    public class UserController : Controller
    {
        private readonly AppDbContext _db;
        public UserController(AppDbContext db) => _db = db;

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var users = await _db.Users.Include(u => u.Role).ToListAsync();
            return View(users);
        }
    }
}
