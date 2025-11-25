using App.Data.Entities;
using App.Eticaret.Models.ViewModels;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Net.Mail;
using System.Security.Claims;

namespace App.Eticaret.Controllers
{
    [AllowAnonymous]
    public class AuthController(IHttpClientFactory clientFactory) : BaseController
    {
        private HttpClient Client => clientFactory.CreateClient("Api.Data");

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
            };

            var response = await Client.PostAsJsonAsync("api/user", user);

            if (!response.IsSuccessStatusCode)
            {
                ModelState.AddModelError(string.Empty, "Kayıt işlemi başarısız. Lütfen tekrar deneyin.");
                return View(newUser);
            }

            SetSuccessMessage("Kayıt işlemi başarılı. Giriş yapabilirsiniz.");

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

            var response = await Client.PostAsJsonAsync("api/user/login", loginModel);

            if (!response.IsSuccessStatusCode)
            {
                ModelState.AddModelError(string.Empty, "Kullanıcı adı veya şifre hatalı.");
                return View(loginModel);
            }

            var user = await response.Content.ReadFromJsonAsync<UserEntity>();

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

            var claims = new List<Claim>
            {
                new(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new(ClaimTypes.Name, user.FirstName),
                new(ClaimTypes.Surname, user.LastName),
                new(ClaimTypes.Email, user.Email),
                new(ClaimTypes.Role, user.Role.Name),
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

            var response = await Client.GetAsync("api/user");

            if (!response.IsSuccessStatusCode)
            {
                ModelState.AddModelError(string.Empty, "Kullanıcı bulunamadı.");
                return View(model);
            }

            var users = await response.Content.ReadFromJsonAsync<List<UserEntity>>();

            if (users == null)
            {
                ModelState.AddModelError(string.Empty, "Kullanıcı bulunamadı.");
                return View(model);
            }

            var user = users.Find(u => u.Email == model.Email);

            if (user == null)
            {
                ModelState.AddModelError(string.Empty, "Kullanıcı bulunamadı.");
                return View(model);
            }

            // Şifre sıfırlama kodu oluşturulacak ve kullanıcıya mail gönderilecek...
            await SendResetPasswordEmailAsync(user);

            SetSuccessMessage("Şifre sıfırlama maili gönderildi. Lütfen e-posta adresinizi kontrol edin.");
            ModelState.Clear();

            return View();
        }

        private async Task SendResetPasswordEmailAsync(UserEntity user)
        {
            // Gönderici mail bilgileri güncellenmeli
            const string host = "smtp.gmail.com";
            const int port = 587;
            const string from = "mail";
            const string password = "şifre";

            var resetPasswordToken = Guid.NewGuid().ToString("n");
            user.ResetPasswordToken = resetPasswordToken;
            var response = await Client.PutAsJsonAsync($"api/user/{user.Id}", user);

            if (!response.IsSuccessStatusCode)
            {
                return;
            }

            using SmtpClient client = new(host, port)
            {
                Credentials = new NetworkCredential(from, password)
            };

            MailMessage mail = new()
            {
                From = new MailAddress(from),
                Subject = "Şifre Sıfırlama",
                Body = $"Merhaba {user.FirstName}, <br> Şifrenizi sıfırlamak için <a href='https://localhost:5001/renew-password/{user.ResetPasswordToken}'>tıklayınız</a>.",
                IsBodyHtml = true,
            };

            mail.To.Add(user.Email);

            await client.SendMailAsync(mail);
        }

        [Route("/renew-password/{verificationCode}")]
        [HttpGet]
        public async Task<IActionResult> RenewPassword([FromRoute] string verificationCode)
        {
            if (!ModelState.IsValid || string.IsNullOrEmpty(verificationCode))
            {
                return RedirectToAction(nameof(ForgotPassword));
            }

            var response = await Client.GetAsync($"api/user/reset-password-token/{verificationCode}");
            if (!response.IsSuccessStatusCode)
            {
                ModelState.AddModelError(string.Empty, "Kullanıcı bulunamadı.");
                return View();
            }

            var user = await response.Content.ReadFromJsonAsync<UserEntity>();

            if (user is null)
            {
                return RedirectToAction(nameof(ForgotPassword));
            }

            return View(new RenewPasswordViewModel
            {
                Email = user.Email,
                Token = verificationCode,
                Password = string.Empty,
                ConfirmPassword = string.Empty,
            });
        }

        [Route("/renew-password")]
        [HttpPost]
        public async Task<IActionResult> RenewPassword([FromForm] RenewPasswordViewModel renewPasswordModel)
        {
            if (!ModelState.IsValid)
            {
                return View(renewPasswordModel);
            }

            var response = await Client.GetAsync($"api/user/reset-password-token/{renewPasswordModel.Token}");
            if (!response.IsSuccessStatusCode)
            {
                ModelState.AddModelError(string.Empty, "Kullanıcı bulunamadı.");
                return View();
            }

            var user = await response.Content.ReadFromJsonAsync<UserEntity>();

            if (user is null)
            {
                return RedirectToAction(nameof(ForgotPassword));
            }

            user.Password = renewPasswordModel.Password;
            user.ResetPasswordToken = string.Empty;

            response = await Client.PutAsJsonAsync($"api/user/{user.Id}", user);
            if (!response.IsSuccessStatusCode)
            {
                ModelState.AddModelError(string.Empty, "Şifre yenilemede bir hatayla karşılaşıldı.");
                return View();
            }

            SetSuccessMessage("Şifreniz başarıyla yenilendi. Giriş yapabilirsiniz.");
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
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        }
    }
}