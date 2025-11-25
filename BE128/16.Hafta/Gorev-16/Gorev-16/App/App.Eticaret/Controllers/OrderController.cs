using App.Eticaret.Models.ViewModels;
using App.Services.Abstract;
using Ardalis.Result;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace App.Eticaret.Controllers
{
    [Authorize(Roles = "buyer, seller")]
    public class OrderController : BaseController
    {
        private readonly IOrderService _orderService;
        public OrderController(IOrderService orderService)
        {
            _orderService = orderService;
        }

        [HttpPost("/order")]
        public async Task<IActionResult> Create([FromForm] string deliveryAddress)
        {
            if (string.IsNullOrWhiteSpace(deliveryAddress))
            {
                ModelState.AddModelError(string.Empty, "Adres gerekli.");
                return RedirectToAction("Checkout", "Cart");
            }
            var jwt = Request.Cookies["AuthToken"] ?? string.Empty;
            var result = await _orderService.PlaceOrderAsync(jwt, new App.Models.DTO.PlaceOrderRequestDto { DeliveryAddress = deliveryAddress });
            if (result.Status == ResultStatus.Unauthorized) return Unauthorized();
            if (!result.IsSuccess) return BadRequest(result.Errors);
            return RedirectToAction("Details", new { orderCode = result.Value.OrderCode });
        }

        [HttpGet("/order/{orderCode}/details")]
        public async Task<IActionResult> Details([FromRoute] string orderCode)
        {
            var jwt = Request.Cookies["AuthToken"] ?? string.Empty;
            var result = await _orderService.GetOrderDetailsByCodeAsync(jwt, orderCode);
            if (result.Status == ResultStatus.Unauthorized) return Unauthorized();
            if (result.Status == ResultStatus.NotFound) return NotFound();
            if (!result.IsSuccess) return BadRequest(result.Errors);
            return View(result.Value);
        }
    }
}