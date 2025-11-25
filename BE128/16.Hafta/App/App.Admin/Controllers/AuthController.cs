using App.Admin.Models.ViewModels;
using App.Data.Entities;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace App.Admin.Controllers
{
    [AllowAnonymous]
    [Route("/auth")]
    public class AuthController(IHttpClientFactory clientFactory) : Controller
    {
        private HttpClient Client => clientFactory.CreateClient("Api.Data");

        [Route("login")]
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [Route("login")]
        [HttpPost]
        public async Task<IActionResult> Login([FromForm] LoginViewModel loginModel)
        {
            if (!ModelState.IsValid)
            {
                return View(loginModel);
            }

            var response = await Client.PostAsJsonAsync("api/user/login", loginModel);

            if (!response.IsSuccessStatusCode)
            {
                ModelState.AddModelError(string.Empty, "Kullanıcı adı veya şifre hatalı.");
                return View(loginModel);
            }

            var user = await response.Content.ReadFromJsonAsync<UserEntity>();

            if (user?.Role?.Name != "admin")
            {
                ModelState.AddModelError(string.Empty, "Bu sayfaya erişim yetkiniz yok.");
                return View(loginModel);
            }

            await DoLoginAsync(user);

            if (Request.Query.ContainsKey("ReturnUrl"))
            {
                return Redirect(Request.Query["ReturnUrl"]!);
            }

            return RedirectToAction(nameof(HomeController.Index), "Home");
        }

        [Route("logout")]
        [HttpGet]
        [Authorize]
        public async Task<IActionResult> Logout()
        {
            await DoLogoutAsync();
            return RedirectToAction(nameof(Login));
        }

        private async Task DoLoginAsync(UserEntity user)
        {
            if (user == null)
            {
                return;
            }

            var claims = new List<Claim>
            {
                new(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new(ClaimTypes.Name, user.FirstName),
                new(ClaimTypes.Surname, user.LastName),
                new(ClaimTypes.Email, user.Email),
                new(ClaimTypes.Role, user.Role.Name),
                new("RoleId", user.RoleId.ToString()),
            };

            var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var principal = new ClaimsPrincipal(identity);
            var authProperties = new AuthenticationProperties
            {
                IsPersistent = true,
                ExpiresUtc = DateTimeOffset.UtcNow.AddMinutes(30),
            };

            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal, authProperties);
        }

        private async Task DoLogoutAsync()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        }
    }
}