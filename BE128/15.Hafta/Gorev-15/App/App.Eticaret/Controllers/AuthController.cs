using App.Eticaret.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http.Json;

namespace App.Eticaret.Controllers
{
    [AllowAnonymous]
    public class AuthController(IHttpClientFactory httpClientFactory) : BaseController
    {
        [Route("/register")]
        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        private record RegisterRequest(string FirstName, string LastName, string Email, string Password);

        [Route("/register")]
        [HttpPost]
        public async Task<IActionResult> Register([FromForm] RegisterUserViewModel newUser)
        {
            if (!ModelState.IsValid)
            {
                return View(newUser);
            }

            var client = httpClientFactory.CreateClient("DataApi");
            var response = await client.PostAsJsonAsync("/api/auth/register", new RegisterRequest(
                newUser.FirstName, newUser.LastName, newUser.Email, newUser.Password));

            if (response.StatusCode == System.Net.HttpStatusCode.Conflict)
            {
                ModelState.AddModelError(string.Empty, "Bu e-posta ile bir kullanıcı zaten mevcut.");
                return View(newUser);
            }

            if (!response.IsSuccessStatusCode)
            {
                ModelState.AddModelError(string.Empty, "Kayıt işlemi tamamlanamadı.");
                return View(newUser);
            }

            ViewBag.SuccessMessage = "Kayıt işlemi başarılı. Giriş yapabilirsiniz.";
            ModelState.Clear();
            return View();
        }

        [Route("/login")]
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        private record LoginRequest(string Email, string Password, string App);
        private record LoginResponse(string Token);

        [Route("/login")]
        [HttpPost]
        public async Task<IActionResult> Login([FromForm] LoginViewModel loginModel)
        {
            if (!ModelState.IsValid)
            {
                return View(loginModel);
            }

            var client = httpClientFactory.CreateClient("DataApi");
            var request = new LoginRequest(loginModel.Email, loginModel.Password, "eticaret");
            var response = await client.PostAsJsonAsync("/api/auth/login", request);
            if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                ModelState.AddModelError(string.Empty, "Kullanıcı adı veya şifre hatalı.");
                return View(loginModel);
            }
            if (!response.IsSuccessStatusCode)
            {
                ModelState.AddModelError(string.Empty, "Giriş yapılamadı.");
                return View(loginModel);
            }

            var payload = await response.Content.ReadFromJsonAsync<LoginResponse>();
            if (payload == null || string.IsNullOrEmpty(payload.Token))
            {
                ModelState.AddModelError(string.Empty, "Geçersiz yanıt.");
                return View(loginModel);
            }

            Response.Cookies.Append("AuthToken", payload.Token, new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.Lax,
                Expires = DateTimeOffset.UtcNow.AddHours(2)
            });

            return RedirectToAction("Index", "Home");
        }

        [Route("/logout")]
        [HttpGet]
        public IActionResult Logout()
        {
            Response.Cookies.Delete("AuthToken");
            return RedirectToAction(nameof(Login));
        }
    }
}