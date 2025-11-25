using App.Data.Entities;
using App.Data.Infrastructure;
using App.Eticaret.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace App.Eticaret.Controllers
{
    [Authorize(Roles = "buyer, seller")]
    public class CartController(DataRepository<ProductEntity> productRepo, DataRepository<CartItemEntity> ciRepo) : BaseController
    {
        [HttpGet("/add-to-cart/{productId:int}")]
        public async Task<IActionResult> AddProduct([FromRoute] int productId)
        {
            var userId = GetUserId();

            if (userId is null)
            {
                return RedirectToAction(nameof(AuthController.Login), "Auth");
            }

            if (!await productRepo.GetAll().AnyAsync(p => p.Id == productId))
            {
                return NotFound();
            }

            var cartItem = await ciRepo.GetAll().FirstOrDefaultAsync(ci => ci.UserId == userId && ci.ProductId == productId);

            if (cartItem is not null)
            {
                cartItem.Quantity++;
            }
            else
            {
                cartItem = new CartItemEntity
                {
                    UserId = userId.Value,
                    ProductId = productId,
                    Quantity = 1,
                    CreatedAt = DateTime.UtcNow
                };

                await ciRepo.AddAsync(cartItem);
            }

            var prevUrl = Request.Headers.Referer.FirstOrDefault();

            if (prevUrl is null)
            {
                return RedirectToAction(nameof(Edit));
            }

            return Redirect(prevUrl);
        }

        [HttpGet("/cart")]
        public async Task<IActionResult> Edit()
        {
            var userId = GetUserId();

            if (userId is null)
            {
                return RedirectToAction(nameof(AuthController.Login), "Auth");
            }

            List<CartItemViewModel> cartItem = await GetCartItemsAsync();

            return View(cartItem);
        }

        [HttpGet("/cart/{cartItemId:int}/remove")]
        public async Task<IActionResult> Remove([FromRoute] int cartItemId)
        {
            var userId = GetUserId();

            if (userId is null)
            {
                return RedirectToAction(nameof(AuthController.Login), "Auth");
            }

            var cartItem = await ciRepo.GetAll().FirstOrDefaultAsync(ci => ci.UserId == userId && ci.Id == cartItemId);

            if (cartItem is null)
            {
                return NotFound();
            }

            await ciRepo.DeleteAsync(cartItemId);

            return RedirectToAction(nameof(Edit));
        }

        [HttpPost("/cart/update")]
        public async Task<IActionResult> UpdateCart(int cartItemId, byte quantity)
        {
            var userId = GetUserId();

            if (userId is null)
            {
                return RedirectToAction(nameof(AuthController.Login), "Auth");
            }

            var cartItem = await ciRepo.GetAll()
                .Include(ci => ci.Product.Images)
                .FirstOrDefaultAsync(ci => ci.UserId == userId && ci.Id == cartItemId);

            if (cartItem is null)
            {
                return NotFound();
            }

            cartItem.Quantity = quantity;
            cartItem = await ciRepo.UpdateAsync(cartItem)!;

            var model = new CartItemViewModel
            {
                Id = cartItem.Id,
                ProductName = cartItem.Product.Name,
                ProductImage = cartItem.Product.Images.Count != 0 ? cartItem.Product.Images.First().Url : null,
                Quantity = cartItem.Quantity,
                Price = cartItem.Product.Price
            };

            return View(model);
        }

        [HttpGet("/checkout")]
        public async Task<IActionResult> Checkout()
        {
            var userId = GetUserId();

            if (userId is null)
            {
                return RedirectToAction(nameof(AuthController.Login), "Auth");
            }

            List<CartItemViewModel> cartItems = await GetCartItemsAsync();

            return View(cartItems);
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