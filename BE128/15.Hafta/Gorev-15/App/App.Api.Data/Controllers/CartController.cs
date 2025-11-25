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
public class CartController(
    DataRepository<CartItemEntity> ciRepo,
    DataRepository<ProductEntity> productRepo) : ControllerBase
{
    [HttpGet("my")]
    public async Task<IActionResult> My()
    {
        var userIdStr = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (!int.TryParse(userIdStr, out var userId)) return Unauthorized();

        var items = await ciRepo.GetAll()
            .Include(ci => ci.Product.Images)
            .Where(ci => ci.UserId == userId)
            .Select(ci => new
            {
                ci.Id,
                ProductName = ci.Product.Name,
                ProductImage = ci.Product.Images.Count != 0 ? ci.Product.Images.First().Url : null,
                ci.Quantity,
                Price = ci.Product.Price
            })
            .ToListAsync();

        return Ok(items);
    }

    [HttpGet("summary")]
    public async Task<IActionResult> Summary()
    {
        var userIdStr = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (!int.TryParse(userIdStr, out var userId)) return Unauthorized();

        var data = await ciRepo.GetAll()
            .Include(ci => ci.Product)
            .Where(ci => ci.UserId == userId)
            .Select(ci => new { ci.Quantity, Price = ci.Product.Price })
            .ToListAsync();

        var count = data.Count;
        var total = data.Sum(x => x.Price * x.Quantity);
        return Ok(new { Count = count, Total = total });
    }

    [HttpPost("add/{productId:int}")]
    public async Task<IActionResult> Add([FromRoute] int productId)
    {
        var userIdStr = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (!int.TryParse(userIdStr, out var userId)) return Unauthorized();

        var productExists = await productRepo.GetAll().AnyAsync(p => p.Id == productId && p.Enabled);
        if (!productExists) return NotFound();

        var cartItem = await ciRepo.GetAll().FirstOrDefaultAsync(ci => ci.UserId == userId && ci.ProductId == productId);
        if (cartItem is not null)
        {
            cartItem.Quantity++;
            await ciRepo.UpdateAsync(cartItem);
            return Ok();
        }
        else
        {
            cartItem = new CartItemEntity
            {
                UserId = userId,
                ProductId = productId,
                Quantity = 1,
                CreatedAt = DateTime.UtcNow
            };
            await ciRepo.AddAsync(cartItem);
            return Ok();
        }
    }

    [HttpDelete("{cartItemId:int}")]
    public async Task<IActionResult> Remove([FromRoute] int cartItemId)
    {
        var userIdStr = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (!int.TryParse(userIdStr, out var userId)) return Unauthorized();

        var cartItem = await ciRepo.GetAll().FirstOrDefaultAsync(ci => ci.UserId == userId && ci.Id == cartItemId);
        if (cartItem is null) return NotFound();

        await ciRepo.DeleteAsync(cartItemId);
        return NoContent();
    }

    public record UpdateCartRequest(byte Quantity);

    [HttpPut("{cartItemId:int}")]
    public async Task<IActionResult> Update([FromRoute] int cartItemId, [FromBody] UpdateCartRequest req)
    {
        var userIdStr = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (!int.TryParse(userIdStr, out var userId)) return Unauthorized();

        var cartItem = await ciRepo.GetAll().Include(ci => ci.Product).FirstOrDefaultAsync(ci => ci.UserId == userId && ci.Id == cartItemId);
        if (cartItem is null) return NotFound();

        cartItem.Quantity = req.Quantity;
        cartItem = await ciRepo.UpdateAsync(cartItem)!;

        return Ok(new
        {
            cartItem.Id,
            ProductName = cartItem.Product.Name,
            ProductImage = cartItem.Product.Images.Count != 0 ? cartItem.Product.Images.First().Url : null,
            cartItem.Quantity,
            Price = cartItem.Product.Price
        });
    }
}
