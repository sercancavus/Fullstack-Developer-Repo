using App.Data.Entities;
using App.Data.Infrastructure;
using App.Eticaret.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace App.Eticaret.Controllers
{
    public class HomeController(ApplicationDbContext dbContext) : BaseController
    {
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet("/about-us")]
        public IActionResult AboutUs()
        {
            return View();
        }

        [HttpGet("/contact")]
        public IActionResult Contact()
        {
            return View();
        }

        [HttpPost("/contact")]
        public async Task<IActionResult> Contact([FromForm] NewContactFormMessageViewModel newContactMessage)
        {
            if (!ModelState.IsValid)
            {
                return View(newContactMessage);
            }

            var contactMessageEntity = new ContactFormEntity
            {
                Name = newContactMessage.Name,
                Email = newContactMessage.Email,
                Message = newContactMessage.Message,
                CreatedAt = DateTime.UtcNow,
                SeenAt = null
            };

            dbContext.ContactForms.Add(contactMessageEntity);
            await dbContext.SaveChangesAsync();

            ViewBag.SuccessMessage = "Your message has been sent successfully.";

            return View();
        }

        [HttpGet("/product/list")]
        public async Task<IActionResult> Listing()
        {
            // TODO: add paging support
            var products = await dbContext.Products
                .Where(p => p.Enabled)
                .Select(p => new ProductListingViewModel
                {
                    Id = p.Id,
                    Name = p.Name,
                    Price = p.Price,
                    CategoryName = p.Category.Name,
                    DiscountPercentage = p.Discount == null ? null : p.Discount.DiscountRate,
                    ImageUrl = p.Images.Count != 0 ? p.Images.First().Url : null
                })
                .ToListAsync();

            return View(products);
        }

        [HttpGet("/product/{productId:int}/details")]
        public async Task<IActionResult> ProductDetail([FromRoute] int productId)
        {
            var product = await dbContext.Products
                .Where(p => p.Enabled && p.Id == productId)
                .Select(p => new HomeProductDetailViewModel
                {
                    Id = p.Id,
                    Name = p.Name,
                    Price = p.Price,
                    DiscountRate = p.Discount == null ? null : p.Discount.DiscountRate,
                    Description = p.Description,
                    StockAmount = p.StockAmount,
                    SellerName = p.Seller.FirstName + " " + p.Seller.LastName,
                    CategoryName = p.Category.Name,
                    CategoryId = p.CategoryId,
                    ImageUrls = p.Images.Select(i => i.Url).ToArray(),
                    Reviews = p.Comments.Where(c => c.IsConfirmed) // show only confirmed comments
                        .Select(c => new ProductReviewViewModel
                        {
                            Id = c.Id,
                            Text = c.Text,
                            StarCount = c.StarCount,
                            UserName = c.User.FirstName + " " + c.User.LastName
                        }).ToArray()
                })
                .FirstOrDefaultAsync();

            if (product is null)
            {
                return NotFound();
            }

            return View(product);
        }
    }
}