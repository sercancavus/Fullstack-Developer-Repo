using App.Mvc.Data;
using App.Mvc.Data.Entities;
using App.Mvc.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace App.Mvc.Controllers
{
    public class AuthController : Controller
    {
        private readonly ILogger<AuthController> _logger;
        private readonly AppDbContext _dbContext;

        public AuthController(ILogger<AuthController> logger, AppDbContext dbContext)
        {
            _logger = logger;
            _dbContext = dbContext;
        }

        //const string UserMail = "admin@admin.com";
        //const string UserPassword = "1234";


        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Login([FromForm] LoginViewModel loginViewModel)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Error = "Invalid login attempt";
                return View();
            }

            //if (loginViewModel.Email != UserMail || loginViewModel.Password != UserPassword)
            //{
            //    ViewBag.Error = "User not found!";
            //    return View();
            //}

            // -------------------------------------------------------------

            //var user = await _dbContext.Users.SingleOrDefaultAsync(x => x.Email == loginViewModel.Email);


            var user = await _dbContext.Users
                .Include(x => x.Role)
                .SingleOrDefaultAsync(x => x.Email == loginViewModel.Email);

            if (user is null || user.Password != loginViewModel.Password)
            {
                ViewBag.Error = "User not found!";
                return View();
            }


            // Giriş başarılı (kullanıcı doğru bilgiler girmiştir.). 
            // TODO: Login işlemi için gereken kodlar yazılacak.

            await DoLoginOperation(user);


            return RedirectToAction("Index", "Home");
        }


        private Task DoLoginOperation(UserEntity user)
        {
            // Cookie içerisinde tutulması istenen veriler, bir Claim listesi şeklinde yazılır.

            // 1)
            // --------------------------------------------------

            //var claims = new List<Claim>
            //{
            //    new Claim(ClaimTypes.Email, loginViewModel.Email),
            //    new Claim(ClaimTypes.Role, "Admin")
            //};



            //var claims = new List<Claim>
            //{
            //    new Claim(ClaimTypes.Email, loginViewModel.Email),
            //    new Claim(ClaimTypes.Role, "Admin"),
            //    new Claim(ClaimTypes.Name, "Siliconmade"),
            //    new Claim("server-time" , DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"))
            //};


            //var claims = new List<Claim>
            //{
            //    new Claim(ClaimTypes.Email, user.Email),
            //    new Claim(ClaimTypes.Name, user.Name),
            //    new Claim(ClaimTypes.Sid, user.Id.ToString()),
            //    new Claim("login-time" , DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"))
            //};

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Name, user.Name),
                new Claim(ClaimTypes.Sid, user.Id.ToString()),
                new Claim(ClaimTypes.Role, user.Role.Name),
                new Claim("login-time" , DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"))
            };


            // 2)
            // Kimlik oluştur.
            // Login olmak isteyen kişi için bir kimlik oluştur.

            var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);


            // 3)
            // Ayarlar

            var authProperties = new AuthenticationProperties
            {
                ExpiresUtc = DateTimeOffset.UtcNow.AddMinutes(10), // Geçerlilik süresi
                IsPersistent = true // beni hatırla işlemlerinde kullanılır.
            };


            // Artık giriş yap!

            // 1. Parametre : Hangi auth şeması kullanılacak.
            // 2. Parametre : Kullanıcının kimliğini temsil eden bir class (ClaimsPrincipal). Kullanıcının bilgilerini içerir.
            // 3. Parametre : Auth özelliklerini taşır.

            return HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, 
                new ClaimsPrincipal(claimsIdentity), authProperties);


        }

        [HttpGet]
        public async Task<IActionResult> Logout()
        {
            // Bu şemayı kullanarak birisi login olduysa, Logout yap
            // Yani Cookie'yi sil
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            return RedirectToAction("Login", "Auth");
        }

        public IActionResult AccessDenied()
        {
            return View();
        }

    }
}
