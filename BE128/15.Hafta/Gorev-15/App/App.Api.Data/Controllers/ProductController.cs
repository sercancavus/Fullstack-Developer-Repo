using App.Data.Entities;
using App.Data.Infrastructure;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace App.Api.Data.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProductController(DataRepository<ProductEntity> productRepo,
    DataRepository<ProductImageEntity> imageRepo,
    DataRepository<ProductCommentEntity> commentRepo) : ControllerBase
{
    [HttpGet("list")] // public listing
    [AllowAnonymous]
    public async Task<IActionResult> List()
    {
        var products = await productRepo.GetAll()
            .Where(p => p.Enabled)
            .Select(p => new
            {
                p.Id,
                p.Name,
                p.Price,
                CategoryName = p.Category.Name,
                DiscountPercentage = p.Discount != null ? (byte?)p.Discount.DiscountRate : null,
                ImageUrl = p.Images.Count != 0 ? p.Images.First().Url : null
            })
            .ToListAsync();
        return Ok(products);
    }

    [HttpPost]
    [Authorize(Roles = "seller")]
    public async Task<IActionResult> Create([FromBody] ProductEntity product)
    {
        product.Id = 0;
        var userIdStr = User.FindFirstValue(System.Security.Claims.ClaimTypes.NameIdentifier);
        if (!int.TryParse(userIdStr, out var userId)) return Unauthorized();
        product.SellerId = userId;
        var created = await productRepo.AddAsync(product);
        return CreatedAtAction(nameof(GetById), new { id = created.Id }, new { created.Id });
    }

    [HttpGet("{id:int}")]
    [AllowAnonymous]
    public async Task<IActionResult> GetById([FromRoute] int id)
    {
        var product = await productRepo.GetAll()
            .Where(p => p.Enabled && p.Id == id)
            .Select(p => new
            {
                p.Id,
                p.Name,
                p.Price,
                DiscountRate = p.Discount != null ? (byte?)p.Discount.DiscountRate : null,
                p.Description,
                p.StockAmount,
                SellerName = p.Seller.FirstName + " " + p.Seller.LastName,
                CategoryName = p.Category.Name,
                CategoryId = p.CategoryId,
                ImageUrls = p.Images.Select(i => i.Url).ToArray(),
                Reviews = p.Comments.Where(c => c.IsConfirmed)
                    .Select(c => new { c.Id, c.Text, c.StarCount, UserName = c.User.FirstName + " " + c.User.LastName })
                    .ToList()
            })
            .FirstOrDefaultAsync();
        if (product is null) return NotFound();
        return Ok(product);
    }

    [HttpPut("{id:int}")]
    [Authorize(Roles = "seller")]
    public async Task<IActionResult> Update([FromRoute] int id, [FromBody] ProductEntity model)
    {
        var entity = await productRepo.GetByIdAsync(id);
        if (entity is null) return NotFound();

        var userIdStr = User.FindFirstValue(System.Security.Claims.ClaimTypes.NameIdentifier);
        if (!int.TryParse(userIdStr, out var userId)) return Unauthorized();
        if (entity.SellerId != userId) return Unauthorized();

        entity.CategoryId = model.CategoryId;
        entity.DiscountId = model.DiscountId;
        entity.Name = model.Name;
        entity.Price = model.Price;
        entity.Description = model.Description;
        entity.StockAmount = model.StockAmount;

        await productRepo.UpdateAsync(entity);
        return NoContent();
    }

    [HttpDelete("{id:int}")]
    [Authorize(Roles = "seller")]
    public async Task<IActionResult> Delete([FromRoute] int id)
    {
        var entity = await productRepo.GetByIdAsync(id);
        if (entity is null) return NotFound();

        var userIdStr = User.FindFirstValue(System.Security.Claims.ClaimTypes.NameIdentifier);
        if (!int.TryParse(userIdStr, out var userId)) return Unauthorized();
        if (entity.SellerId != userId) return Unauthorized();

        await productRepo.DeleteAsync(id);
        return NoContent();
    }

    [HttpPost("{productId:int}/comment")]
    [Authorize(Roles = "buyer,seller")]
    public async Task<IActionResult> Comment([FromRoute] int productId, [FromBody] ProductCommentEntity newComment)
    {
        var userIdStr = User.FindFirstValue(System.Security.Claims.ClaimTypes.NameIdentifier);
        if (!int.TryParse(userIdStr, out var userId)) return Unauthorized();

        if (!await productRepo.GetAll().AnyAsync(x => x.Id == productId)) return NotFound();
        if (await commentRepo.GetAll().AnyAsync(x => x.ProductId == productId && x.UserId == userId)) return BadRequest();

        newComment.Id = 0;
        newComment.ProductId = productId;
        newComment.UserId = userId;
        await commentRepo.AddAsync(newComment);
        return Ok();
    }

    public record AddImageRequest(string Url);

    [HttpPost("{productId:int}/images")]
    [Authorize(Roles = "seller")]
    public async Task<IActionResult> AddImage([FromRoute] int productId, [FromBody] AddImageRequest request)
    {
        var entity = await productRepo.GetByIdAsync(productId);
        if (entity is null) return NotFound();

        var userIdStr = User.FindFirstValue(System.Security.Claims.ClaimTypes.NameIdentifier);
        if (!int.TryParse(userIdStr, out var userId)) return Unauthorized();
        if (entity.SellerId != userId) return Unauthorized();

        var img = new ProductImageEntity
        {
            ProductId = productId,
            Url = request.Url
        };
        var created = await imageRepo.AddAsync(img);
        return Created($"/api/product/{productId}/images/{created.Id}", new { created.Id });
    }
}
