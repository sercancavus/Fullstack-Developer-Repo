using App.Data.Entities;
using App.Data.Infrastructure;
using App.Eticaret.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace App.Eticaret.Controllers
{
    public class AuthController(ApplicationDbContext dbContext) : BaseController
    {
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

            var user = new UserEntity
            {
                FirstName = newUser.FirstName,
                LastName = newUser.LastName,
                Email = newUser.Email,
                Password = newUser.Password,
                RoleId = 3,
                CreatedAt = DateTime.UtcNow,
            };

            dbContext.Users.Add(user);
            await dbContext.SaveChangesAsync();

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

            var user = await dbContext.Users.FirstOrDefaultAsync(u => u.Email == loginModel.Email && u.Password == loginModel.Password);

            if (user == null)
            {
                ModelState.AddModelError(string.Empty, "Kullanıcı adı veya şifre hatalı.");
                return View(loginModel);
            }

            await LogInAsync(user);

            return RedirectToAction("Index", "Home");
        }

        private async Task LogInAsync(UserEntity user)
        {
            if (user == null)
            {
                return;
            }

            SetCookie("userId", user.Id.ToString());
            SetCookie("mail", user.Email);
            SetCookie("name", user.FirstName);
            SetCookie("surname", user.LastName);
            SetCookie("role", user.RoleId.ToString());

            await Task.CompletedTask;
        }

        [Route("/forgot-password")]
        [HttpGet]
        public IActionResult ForgotPassword()
        {
            return View();
        }

        [Route("/forgot-password")]
        [HttpPost]
        public async Task<IActionResult> ForgotPassword([FromForm] ForgotPasswordViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var user = await dbContext.Users.FirstOrDefaultAsync(u => u.Email == model.Email);

            if (user == null)
            {
                ModelState.AddModelError(string.Empty, "Kullanıcı bulunamadı.");
                return View(model);
            }

            // Şifre sıfırlama kodu oluşturulacak ve kullanıcıya mail gönderilecek...
            await SendResetPasswordEmailAsync(user);

            ViewBag.SuccessMessage = "Şifre sıfırlama maili gönderildi. Lütfen e-posta adresinizi kontrol edin.";
            ModelState.Clear();

            return View();
        }

        private async Task SendResetPasswordEmailAsync(UserEntity user)
        {
            // Şifre sıfırlama maili gönderme kodları...
            // TODO: Authorization implemente edildikten sonra bu metot tamamlanacak...
            if (user == null)
            {
                return;
            }

            await Task.CompletedTask;
        }

        [Route("/renew-password/{verificationCode}")]
        [HttpGet]
        public async Task<IActionResult> RenewPassword([FromRoute] string verificationCode)
        {
            // TODO: Authorization implemente edildikten sonra bu metot tamamlanacak...
            return View();
        }

        [Route("/renew-password")]
        [HttpPost]
        public async Task<IActionResult> RenewPassword([FromForm] object changePasswordModel)
        {
            // TODO: Authorization implemente edildikten sonra bu metot tamamlanacak...
            return View();
        }

        [Route("/logout")]
        [HttpGet]
        public async Task<IActionResult> Logout()
        {
            await LogoutUser();

            return RedirectToAction(nameof(Login));
        }

        private async Task LogoutUser()
        {
            // TODO: Authorization implemente edildikten sonra bu metot tamamlanacak...

            RemoveCookie("userId");
            RemoveCookie("mail");
            RemoveCookie("name");
            RemoveCookie("surname");
            RemoveCookie("role");
        }
    }
}