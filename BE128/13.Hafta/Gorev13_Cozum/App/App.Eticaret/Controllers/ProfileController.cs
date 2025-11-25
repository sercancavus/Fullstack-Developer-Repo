using App.Data.Infrastructure;
using App.Eticaret.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace App.Eticaret.Controllers
{
    public class ProfileController(ApplicationDbContext dbContext) : BaseController
    {
        [HttpGet("/profile")]
        public async Task<IActionResult> Details()
        {
            var userId = GetUserId();

            if (userId is null)
            {
                return RedirectToAction("Login", "Auth");
            }

            var userViewModel = await dbContext.Users
                .Where(u => u.Id == userId.Value)
                .Select(u => new ProfileDetailsViewModel
                {
                    FirstName = u.FirstName,
                    LastName = u.LastName,
                    Email = u.Email,

                })
                .FirstOrDefaultAsync();

            if (userViewModel is null)
            {
                return RedirectToAction("Login", "Auth");
            }

            string? previousSuccessMessage = TempData["SuccessMessage"]?.ToString();

            if (previousSuccessMessage is not null)
            {
                SetSuccessMessage(previousSuccessMessage);
            }

            return View(userViewModel);
        }

        [HttpPost("/profile")]
        public async Task<IActionResult> Edit([FromForm] ProfileDetailsViewModel editMyProfileModel)
        {
            if (!IsUserLoggedIn())
            {
                return RedirectToAction("Login", "Auth");
            }

            var user = await GetCurrentUserAsync();

            if (user is null)
            {
                return RedirectToAction("Login", "Auth");
            }

            if (!ModelState.IsValid)
            {
                return View(editMyProfileModel);
            }

            user.FirstName = editMyProfileModel.FirstName;
            user.LastName = editMyProfileModel.LastName;

            if (!string.IsNullOrWhiteSpace(editMyProfileModel.Password) && editMyProfileModel.Password != "******")
            {
                user.Password = editMyProfileModel.Password;
            }

            await dbContext.SaveChangesAsync();

            TempData["SuccessMessage"] = "Profiliniz başarıyla güncellendi.";

            return RedirectToAction(nameof(Details));
        }

        [HttpGet("/my-orders")]
        public async Task<IActionResult> MyOrders()
        {
            var userId = GetUserId();

            if (userId is null)
            {
                return RedirectToAction("Login", "Auth");
            }

            List<OrderViewModel> orders = await dbContext.Orders
                .Where(o => o.UserId == userId.Value)
                .Select(o => new OrderViewModel
                {
                    OrderCode = o.OrderCode,
                    Address = o.Address,
                    CreatedAt = o.CreatedAt,
                    TotalPrice = o.OrderItems.Sum(oi => oi.UnitPrice * oi.Quantity),
                    TotalProducts = o.OrderItems.Count,
                    TotalQuantity = o.OrderItems.Sum(oi => oi.Quantity),
                })
                .OrderByDescending(o => o.CreatedAt)
                .ToListAsync();

            return View(orders);
        }

        [HttpGet("/my-products")]
        public async Task<IActionResult> MyProducts()
        {

            var userId = GetUserId();

            if (userId is null)
            {
                return RedirectToAction("Login", "Auth");
            }

            if (!await IsUserSellerAsync())
            {
                return RedirectToAction("Index", "Home");
            }

            List<MyProductsViewModel> products = await dbContext.Products
                .Where(p => p.SellerId == userId.Value)
                .Select(p => new MyProductsViewModel
                {
                    Id = p.Id,
                    Name = p.Name,
                    Price = p.Price,
                    Description = p.Description,
                    Stock = p.StockAmount,
                    HasDiscount = p.DiscountId != null,
                    CreatedAt = p.CreatedAt,
                })
                .OrderByDescending(p => p.CreatedAt)
                .ToListAsync();

            return View(products);
        }
    }
}