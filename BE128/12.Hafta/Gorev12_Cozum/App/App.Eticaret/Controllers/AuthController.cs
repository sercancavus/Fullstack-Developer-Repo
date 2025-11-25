using App.Eticaret.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace App.Eticaret.Controllers
{
    public class AuthController : Controller
    {
        [Route("/register")]
        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

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

        [Route("/forgot-password")]
        [HttpGet]
        public IActionResult ForgotPassword()
        {
            return View();
        }

        [Route("/forgot-password")]
        [HttpPost]
        public IActionResult ForgotPassword([FromForm] object forgotPasswordMailModel)
        {
            return View();
        }

        [Route("/renew-password/{verificationCode}")]
        [HttpGet]
        public IActionResult RenewPassword([FromRoute] string verificationCode)
        {
            return View();
        }

        [Route("/renew-password")]
        [HttpPost]
        public IActionResult RenewPassword([FromForm] object changePasswordModel)
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