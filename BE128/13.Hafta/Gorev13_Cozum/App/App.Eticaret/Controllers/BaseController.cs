using App.Data.Entities;
using App.Data.Infrastructure;
using Microsoft.AspNetCore.Mvc;

namespace App.Eticaret.Controllers
{
    public abstract class BaseController : Controller
    {
        protected T? GetService<T>() where T : class => HttpContext.RequestServices.GetService<T>();

        protected void SetSuccessMessage(string message) => ViewBag.SuccessMessage = message;

        protected void SetErrorMessage(string message) => ViewBag.ErrorMessage = message;

        protected string? GetCookie(string key) => Request.Cookies[key];

        protected void SetCookie(string key, string value) => Response.Cookies.Append(key, value);

        protected void RemoveCookie(string key) => Response.Cookies.Delete(key);

        protected async Task<UserEntity?> GetCurrentUserAsync()
        {
            var userIdStr = GetCookie("userId");

            if (!int.TryParse(userIdStr, out int userId))
            {
                return null;
            }

            var dbContext = GetService<ApplicationDbContext>();

            if (dbContext == null)
            {
                return null;
            }

            return await dbContext.Users.FindAsync(userId);
        }

        protected bool IsUserLoggedIn() => GetCookie("userId") != null;

        protected int? GetUserId() => int.TryParse(GetCookie("userId"), out int userId) ? userId : null;

        protected async Task<bool> IsUserAdminAsync()
        {
            var user = await GetCurrentUserAsync();

            return user?.RoleId == 1;
        }

        protected async Task<bool> IsUserSellerAsync()
        {
            var user = await GetCurrentUserAsync();

            return user?.RoleId == 2;
        }

        protected async Task<bool> IsUserBuyerAsync()
        {
            var user = await GetCurrentUserAsync();

            return user?.RoleId == 3;
        }
    }
}
