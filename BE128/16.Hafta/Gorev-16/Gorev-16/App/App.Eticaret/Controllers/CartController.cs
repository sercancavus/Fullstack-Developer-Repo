using App.Eticaret.Models.ViewModels;
using App.Services.Abstract;
using Ardalis.Result;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace App.Eticaret.Controllers
{
    [Authorize(Roles = "buyer, seller")]
    public class CartController : BaseController
    {
        private readonly ICartService _cartService;
        public CartController(ICartService cartService)
        {
            _cartService = cartService;
        }

        [HttpGet("/add-to-cart/{productId:int}")]
        public async Task<IActionResult> AddProduct([FromRoute] int productId)
        {
            var jwt = Request.Cookies["AuthToken"] ?? string.Empty;
            var result = await _cartService.AddToCartAsync(jwt, productId);
            if (result.Status == ResultStatus.NotFound)
                return NotFound();
            if (result.Status == ResultStatus.Unauthorized)
                return Unauthorized();
            var prevUrl = Request.Headers.Referer.FirstOrDefault();
            return prevUrl is null ? RedirectToAction(nameof(Edit)) : Redirect(prevUrl);
        }

        [HttpGet("/cart")]
        public async Task<IActionResult> Edit()
        {
            var jwt = Request.Cookies["AuthToken"] ?? string.Empty;
            var result = await _cartService.GetMyCartAsync(jwt);
            if (result.Status == ResultStatus.Unauthorized)
                return Unauthorized();
            // TODO: DTO'dan ViewModel'e map
            return View(result.Value ?? new List<App.Models.DTO.CartItemDto>());
        }

        [HttpGet("/cart/{cartItemId:int}/remove")]
        public async Task<IActionResult> Remove([FromRoute] int cartItemId)
        {
            var jwt = Request.Cookies["AuthToken"] ?? string.Empty;
            var result = await _cartService.RemoveFromCartAsync(jwt, cartItemId);
            if (result.Status == ResultStatus.NotFound)
                return NotFound();
            if (result.Status == ResultStatus.Unauthorized)
                return Unauthorized();
            return RedirectToAction(nameof(Edit));
        }

        [HttpPost("/cart/update")]
        public async Task<IActionResult> UpdateCart(int cartItemId, byte quantity)
        {
            var jwt = Request.Cookies["AuthToken"] ?? string.Empty;
            var updateRequest = new App.Models.DTO.UpdateCartItemRequestDto { CartItemId = cartItemId, Quantity = quantity };
            var result = await _cartService.UpdateCartItemAsync(jwt, updateRequest);
            if (result.Status == ResultStatus.NotFound)
                return NotFound();
            if (result.Status == ResultStatus.Unauthorized)
                return Unauthorized();
            // TODO: DTO'dan ViewModel'e map
            return View(result.Value);
        }

        [HttpGet("/checkout")]
        public async Task<IActionResult> Checkout()
        {
            var jwt = Request.Cookies["AuthToken"] ?? string.Empty;
            var result = await _cartService.GetMyCartAsync(jwt);
            if (result.Status == ResultStatus.Unauthorized)
                return Unauthorized();
            // TODO: DTO'dan ViewModel'e map
            return View(result.Value ?? new List<App.Models.DTO.CartItemDto>());
        }
    }
}