using App.Data.Entities;
using App.Data.Infrastructure;
using App.Eticaret.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace App.Eticaret.Controllers
{
    [Authorize(Roles = "buyer, seller")]
    public class OrderController(DataRepository<CartItemEntity> ciRepo,
        DataRepository<OrderEntity> orRepo,
        DataRepository<OrderItemEntity> oiRepo) : BaseController
    {
        [HttpPost("/order")]
        public async Task<IActionResult> Create([FromForm] CheckoutViewModel model)
        {
            var userId = GetUserId();

            if (userId is null)
            {
                return RedirectToAction(nameof(AuthController.Login), "Auth");
            }

            if (!ModelState.IsValid)
            {
                var viewModel = await GetCartItemsAsync();
                return View(viewModel);
            }

            var cartItems = await ciRepo.GetAll()
                .Include(ci => ci.Product)
                .Where(ci => ci.UserId == userId)
                .ToListAsync();

            if (cartItems.Count == 0)
            {
                return RedirectToAction(nameof(CartController.Edit), "Cart");
            }

            var order = new OrderEntity
            {
                UserId = userId.Value,
                Address = model.Address,
                OrderCode = await CreateOrderCode(),
            };

            order = await orRepo.AddAsync(order);

            foreach (var cartItem in cartItems)
            {
                var orderItem = new OrderItemEntity
                {
                    OrderId = order.Id,
                    ProductId = cartItem.ProductId,
                    Quantity = cartItem.Quantity,
                    UnitPrice = cartItem.Product.Price,
                };

                await oiRepo.AddAsync(orderItem);
            }

            return RedirectToAction(nameof(Details), new { orderCode = order.OrderCode });
        }

        [HttpGet("/order/{orderCode}/details")]
        public async Task<IActionResult> Details([FromRoute] string orderCode)
        {
            var userId = GetUserId();

            if (userId is null)
            {
                return RedirectToAction(nameof(AuthController.Login), "Auth");
            }

            var order = await orRepo.GetAll()
                .Where(o => o.UserId == userId && o.OrderCode == orderCode)
                .Select(o => new OrderDetailsViewModel
                {
                    OrderCode = o.OrderCode,
                    CreatedAt = o.CreatedAt,
                    Address = o.Address,
                    Items = o.OrderItems.Select(oi => new OrderItemViewModel
                    {
                        ProductName = oi.Product.Name,
                        Quantity = oi.Quantity,
                        UnitPrice = oi.UnitPrice,
                    }).ToList()
                })
                .FirstOrDefaultAsync();

            if (order is null)
            {
                return NotFound();
            }

            return View(order);
        }

        private async Task<string> CreateOrderCode()
        {
            var orderCode = Guid.NewGuid().ToString("n")[..16].ToUpperInvariant();
            while (await orRepo.GetAll().AnyAsync(x => x.OrderCode == orderCode))
            {
                orderCode = Guid.NewGuid().ToString("n")[..16].ToUpperInvariant();
            }

            return orderCode;
        }

        private async Task<List<CartItemViewModel>> GetCartItemsAsync()
        {
            var userId = GetUserId() ?? -1;

            return await ciRepo.GetAll()
                .Include(ci => ci.Product.Images)
                .Where(ci => ci.UserId == userId)
                .Select(ci => new CartItemViewModel
                {
                    Id = ci.Id,
                    ProductName = ci.Product.Name,
                    ProductImage = ci.Product.Images.Count != 0 ? ci.Product.Images.First().Url : null,
                    Quantity = ci.Quantity,
                    Price = ci.Product.Price
                })
                .ToListAsync();
        }
    }
}