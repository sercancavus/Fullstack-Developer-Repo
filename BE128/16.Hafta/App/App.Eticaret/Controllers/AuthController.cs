using App.Eticaret.Models.ViewModels;
using App.Models.DTO.Auth;
using App.Services.Abstract;
using Ardalis.Result;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace App.Eticaret.Controllers
{
    [AllowAnonymous]
    public class AuthController(IAuthService authService, IMapper mapper)
        : BaseController
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

            var dto = mapper.Map<AuthRegisterRequest>(newUser);

            var result = await authService.RegisterAsync(dto);

            if (!result.IsSuccess)
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
            var dto = mapper.Map<AuthLoginRequest>(loginModel);

            var result = await authService.LoginAsync(dto);

            if (!result.IsSuccess)
            {
                ModelState.AddModelError(string.Empty, "Kullanıcı adı veya şifre hatalı.");
                return View(loginModel);
            }

            if (result.Value?.Token is null)
            {
                ModelState.AddModelError(string.Empty, "Kullanıcı adı veya şifre hatalı.");
                return View(loginModel);
            }

            LogInAsync(result.Value.Token);

            return RedirectToAction("Index", "Home");
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

            var dto = mapper.Map<AuthForgotPasswordRequest>(model);

            var result = await authService.ForgotPasswordAsync(dto);

            if (result.Status == ResultStatus.NotFound)
            {
                ModelState.AddModelError(string.Empty, "Kullanıcı bulunamadı.");
                return View(model);
            }

            if (!result.IsSuccess)
            {
                ModelState.AddModelError(string.Empty, "Mail gönderilemedi.");
                return View(model);
            }

            SetSuccessMessage("Şifre sıfırlama maili gönderildi. Lütfen e-posta adresinizi kontrol edin.");
            ModelState.Clear();

            return View();
        }

        [Route("/renew-password/{verificationCode}")]
        [HttpGet]
        public IActionResult RenewPassword([FromRoute] string verificationCode)
        {
            if (!ModelState.IsValid || string.IsNullOrEmpty(verificationCode))
            {
                return RedirectToAction(nameof(ForgotPassword));
            }

            return View(new RenewPasswordViewModel
            {
                Email = string.Empty,
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

            var dto = mapper.Map<AuthResetPasswordRequest>(renewPasswordModel);

            var result = await authService.ResetPasswordAsync(dto);

            if (result.Status == ResultStatus.NotFound)
            {
                ModelState.AddModelError(string.Empty, "Kullanıcı bulunamadı.");
                return View(renewPasswordModel);
            }

            if (!result.IsSuccess)
            {
                ModelState.AddModelError(string.Empty, "Şifre yenilemede bir hatayla karşılaşıldı.");
                return View(renewPasswordModel);
            }

            SetSuccessMessage("Şifreniz başarıyla yenilendi. Giriş yapabilirsiniz.");
            ModelState.Clear();
            return View();
        }

        [Route("/logout")]
        [HttpGet]
        public async Task<IActionResult> Logout()
        {
            await LogoutUser();

            return RedirectToAction(nameof(Login));
        }

        private void LogInAsync(string token)
        {
            Response.Cookies.Append("auth-token", token);
        }

        private async Task LogoutUser()
        {
            Response.Cookies.Delete("auth-token");
            await Task.CompletedTask;
        }
    }
}