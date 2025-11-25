using App.Admin.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace App.Admin.Controllers
{
    public class AuthController : Controller
    {
        [Route("/login")]
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [Route("/login")]
        [HttpPost]
        public IActionResult Login([FromForm] LoginViewModel loginModel)
        {
            return View();
        }

        [Route("/logout")]
        [HttpGet]
        public IActionResult Logout()
        {
            // logout kodları...

            return RedirectToAction(nameof(Login));
        }
    }
}