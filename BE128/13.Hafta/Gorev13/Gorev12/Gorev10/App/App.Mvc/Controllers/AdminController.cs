using App.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace App.Mvc.Controllers
{
    [Authorize(Roles = "admin")]
    public class AdminController : Controller
    {
        private readonly AppDbContext _db;
        public AdminController(AppDbContext db) => _db = db;

        public IActionResult Index()
        {
            // Burada ürün, kullanýcý, sipariþ özetleri gösterilebilir
            return View();
        }

        // Diðer admin iþlemleri (ürün yönetimi, kullanýcý yönetimi, sipariþ yönetimi) burada olacak
    }
}
