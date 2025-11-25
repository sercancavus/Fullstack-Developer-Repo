using App.Data.Entities;
using App.Data.Infrastructure;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace App.Api.Data.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize(Roles = "buyer,seller")]
public class OrderController(DataRepository<OrderEntity> orderRepo, DataRepository<CartItemEntity> cartRepo, DataRepository<OrderItemEntity> orderItemRepo) : ControllerBase
{
    [HttpGet("my")]
    public async Task<IActionResult> My()
    {
        var userIdStr = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (!int.TryParse(userIdStr, out var userId)) return Unauthorized();

        var orders = await orderRepo.GetAll()
            .Where(o => o.UserId == userId)
            .Select(o => new
            {
                o.OrderCode,
                o.Address,
                o.CreatedAt,
                TotalPrice = o.OrderItems.Sum(oi => oi.UnitPrice * oi.Quantity),
                TotalProducts = o.OrderItems.Count,
                TotalQuantity = o.OrderItems.Sum(oi => oi.Quantity)
            })
            .OrderByDescending(o => o.CreatedAt)
            .ToListAsync();

        return Ok(orders);
    }

    [HttpGet("{orderCode}/details")]
    public async Task<IActionResult> Details([FromRoute] string orderCode)
    {
        var userIdStr = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (!int.TryParse(userIdStr, out var userId)) return Unauthorized();

        var order = await orderRepo.GetAll()
            .Where(o => o.UserId == userId && o.OrderCode == orderCode)
            .Select(o => new
            {
                o.OrderCode,
                o.CreatedAt,
                o.Address,
                Items = o.OrderItems.Select(oi => new
                {
                    ProductName = oi.Product.Name,
                    oi.Quantity,
                    oi.UnitPrice
                }).ToList()
            })
            .FirstOrDefaultAsync();

        if (order is null) return NotFound();
        return Ok(order);
    }

    public record PlaceOrderRequest(string DeliveryAddress);

    [HttpPost]
    public async Task<IActionResult> Place([FromBody] PlaceOrderRequest request)
    {
        if (string.IsNullOrWhiteSpace(request.DeliveryAddress)) return BadRequest();

        var userIdStr = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (!int.TryParse(userIdStr, out var userId)) return Unauthorized();

        var cartItems = await cartRepo.GetAll()
            .Include(ci => ci.Product)
            .Where(ci => ci.UserId == userId)
            .ToListAsync();

        if (cartItems.Count == 0) return BadRequest(new { message = "Sepet boþ" });

        var orderCode = Guid.NewGuid().ToString("N").Substring(0, 10).ToUpperInvariant();
        var order = new OrderEntity
        {
            UserId = userId,
            OrderCode = orderCode,
            Address = request.DeliveryAddress,
            CreatedAt = DateTime.UtcNow
        };
        order = await orderRepo.AddAsync(order);

        foreach (var ci in cartItems)
        {
            await orderItemRepo.AddAsync(new OrderItemEntity
            {
                OrderId = order.Id,
                ProductId = ci.ProductId,
                Quantity = ci.Quantity,
                UnitPrice = ci.Product.Price,
                CreatedAt = DateTime.UtcNow
            });
        }

        // Clear cart
        foreach (var ci in cartItems)
        {
            await cartRepo.DeleteAsync(ci.Id);
        }

        return Created($"/api/order/{orderCode}/details", new { OrderCode = orderCode });
    }
}
