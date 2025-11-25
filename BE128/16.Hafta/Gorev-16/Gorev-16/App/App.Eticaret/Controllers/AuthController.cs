using App.Eticaret.Models.ViewModels;
using App.Services.Abstract;
using Ardalis.Result;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace App.Eticaret.Controllers
{
    [AllowAnonymous]
    public class AuthController : BaseController
    {
        private readonly IAuthService _authService;
        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [Route("/register")]
        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [Route("/register")]
        [HttpPost]
        public async Task<IActionResult> Register([FromForm] RegisterUserViewModel newUser)
        {
            if (!ModelState.IsValid)
            {
                return View(newUser);
            }

            var dto = new App.Models.DTO.RegisterRequestDto
            {
                FirstName = newUser.FirstName,
                LastName = newUser.LastName,
                Email = newUser.Email,
                Password = newUser.Password
            };
            var result = await _authService.RegisterAsync(dto);
            if (!result.IsSuccess)
            {
                ModelState.AddModelError(string.Empty, result.Errors.FirstOrDefault() ?? "Kayıt işlemi tamamlanamadı.");
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

        [Route("/login")]
        [HttpPost]
        public async Task<IActionResult> Login([FromForm] LoginViewModel loginModel)
        {
            if (!ModelState.IsValid)
            {
                return View(loginModel);
            }

            var dto = new App.Models.DTO.LoginRequestDto
            {
                Email = loginModel.Email,
                Password = loginModel.Password,
                App = "eticaret"
            };
            var result = await _authService.LoginAsync(dto);
            if (result.Status == ResultStatus.NotFound)
            {
                ModelState.AddModelError(string.Empty, "Kullanıcı adı veya şifre hatalı.");
                return View(loginModel);
            }
            if (!result.IsSuccess)
            {
                ModelState.AddModelError(string.Empty, result.Errors.FirstOrDefault() ?? "Giriş yapılamadı.");
                return View(loginModel);
            }
            var payload = result.Value;
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