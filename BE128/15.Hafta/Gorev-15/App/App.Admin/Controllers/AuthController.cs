using App.Admin.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http.Json;

namespace App.Admin.Controllers
{
    [AllowAnonymous]
    public class AuthController(IHttpClientFactory httpClientFactory) : Controller
    {
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
            var response = await client.PostAsJsonAsync("/api/auth/login", new LoginRequest(loginModel.Email, loginModel.Password, "admin"));
            if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                ModelState.AddModelError(string.Empty, "Kullanıcı adı veya şifre hatalı.");
                return View(loginModel);
            }
            if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
            {
                ModelState.AddModelError(string.Empty, "Bu sayfaya erişim yetkiniz yok.");
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