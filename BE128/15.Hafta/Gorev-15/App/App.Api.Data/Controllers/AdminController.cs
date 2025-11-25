using App.Data.Entities;
using App.Data.Infrastructure;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace App.Api.Data.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize(Roles = "admin")]
public class AdminController(
    DataRepository<UserEntity> userRepo,
    DataRepository<ProductEntity> productRepo,
    DataRepository<ProductCommentEntity> commentRepo) : ControllerBase
{
    // Users
    [HttpGet("users")] 
    public async Task<IActionResult> Users()
    {
        var users = await userRepo.GetAll()
            .Where(u => u.RoleId != 1)
            .Select(u => new
            {
                u.Id,
                u.FirstName,
                u.LastName,
                u.Email,
                Role = u.Role.Name,
                u.Enabled,
                u.HasSellerRequest
            })
            .ToListAsync();
        return Ok(users);
    }

    [HttpPost("users/{id:int}/enable")] public async Task<IActionResult> Enable([FromRoute] int id)
    {
        var user = await userRepo.GetByIdAsync(id);
        if (user is null) return NotFound();
        user.Enabled = true; await userRepo.UpdateAsync(user); return NoContent();
    }

    [HttpPost("users/{id:int}/disable")] public async Task<IActionResult> Disable([FromRoute] int id)
    {
        var user = await userRepo.GetByIdAsync(id);
        if (user is null) return NotFound();
        user.Enabled = false; await userRepo.UpdateAsync(user); return NoContent();
    }

    [HttpPost("users/{id:int}/approve-seller")] public async Task<IActionResult> ApproveSeller([FromRoute] int id)
    {
        var user = await userRepo.GetByIdAsync(id);
        if (user is null) return NotFound();
        if (!user.HasSellerRequest) return BadRequest();
        user.HasSellerRequest = false; user.RoleId = 2; await userRepo.UpdateAsync(user); return NoContent();
    }

    // Comments
    [HttpGet("comments/pending")] 
    public async Task<IActionResult> PendingComments()
    {
        var comments = await commentRepo.GetAll()
            .Where(c => !c.IsConfirmed)
            .Select(c => new { c.Id, c.Text, c.StarCount, ProductName = c.Product.Name, UserName = c.User.FirstName + " " + c.User.LastName })
            .ToListAsync();
        return Ok(comments);
    }

    [HttpPost("comments/{commentId:int}/approve")] public async Task<IActionResult> ApproveComment(int commentId)
    {
        var comment = await commentRepo.GetByIdAsync(commentId);
        if (comment is null) return NotFound();
        comment.IsConfirmed = true; await commentRepo.UpdateAsync(comment); return NoContent();
    }

    // Products
    [HttpGet("products")] 
    public async Task<IActionResult> Products()
    {
        var products = await productRepo.GetAll()
            .Select(p => new { p.Id, p.Name, p.Price, p.Enabled, Seller = p.Seller.FirstName + " " + p.Seller.LastName })
            .ToListAsync();
        return Ok(products);
    }

    [HttpPost("products/{id:int}/disable")] public async Task<IActionResult> DisableProduct(int id)
    {
        var p = await productRepo.GetByIdAsync(id); if (p is null) return NotFound(); p.Enabled = false; await productRepo.UpdateAsync(p); return NoContent();
    }

    [HttpPost("products/{id:int}/enable")] public async Task<IActionResult> EnableProduct(int id)
    {
        var p = await productRepo.GetByIdAsync(id); if (p is null) return NotFound(); p.Enabled = true; await productRepo.UpdateAsync(p); return NoContent();
    }
}
