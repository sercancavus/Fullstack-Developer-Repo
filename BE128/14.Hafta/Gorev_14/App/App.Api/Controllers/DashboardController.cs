using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace App.Api.Controllers
{
    [Authorize]
    public class DashboardController : Controller
    {
        [Authorize(Roles = "Seller")]
        public IActionResult Seller()
        {
            return View();
        }

        [Authorize(Roles = "Buyer")]
        public IActionResult Buyer()
        {
            return View();
        }
    }
}