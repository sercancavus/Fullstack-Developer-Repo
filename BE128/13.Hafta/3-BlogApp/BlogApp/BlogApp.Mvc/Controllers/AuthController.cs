using BlogApp.Data.Entities;
using BlogApp.Mvc.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace BlogApp.Mvc.Controllers
{
    public class AuthController : Controller
    {
        private readonly DbContext _dbContext;
        private readonly IConfiguration _config;

        public AuthController(DbContext dbContext, IConfiguration config)
        {
            _dbContext = dbContext;
            _config = config;
        }

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
                return View();
            }

            var dbSet = _dbContext.Set<UserEntity>();
            var user = dbSet.FirstOrDefault(u => u.Email == loginViewModel.Email && u.Password == loginViewModel.Password);

            if (user is null)
            {
                ViewBag.Error = "Invalid credentials";
                return View();
            }

            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Email,user.Email ),
                new Claim(JwtRegisteredClaimNames.Sub,user.Id.ToString())
            };

            var symmetricKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Secret"]));

            var tokenoptions = new JwtSecurityToken(
                issuer: "BlogApp",
                audience: "BlogApp",
                claims: claims,
                expires: DateTime.Now.AddMinutes(5),
                signingCredentials: new SigningCredentials(symmetricKey, SecurityAlgorithms.HmacSha256)
                );

            string tokenString = new JwtSecurityTokenHandler().WriteToken(tokenoptions);

            Response.Cookies.Append("access_token", tokenString, new CookieOptions
            {
                HttpOnly = true, // js ile erişilmesin
                Secure = true, // https kullanabilsin
                SameSite = SameSiteMode.Strict // Sadece bu sitede kullanılabilsin.
            });

            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public async Task<IActionResult> Logout()
        {
            //await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            Response.Cookies.Delete("access_token");

            return RedirectToAction("Login", "Auth");
        }

    }
}
