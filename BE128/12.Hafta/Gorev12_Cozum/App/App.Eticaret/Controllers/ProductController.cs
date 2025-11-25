using App.Data.Contexts;
using App.Eticaret.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace App.Eticaret.Controllers
{
    public class ProductController : Controller
    {
        private readonly ApplicationDbContext _dbContext;

        public ProductController(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        [Route("/product")]
        [HttpGet]
        public async Task<IActionResult> Create()
        {
            ViewBag.Discounts = await _dbContext.ProductDiscounts
                .Where(d => d.Enabled)
                .Select(d => new DiscountSelectItemViewModel { Id = d.Id, Rate = d.DiscountRate })
                .ToListAsync();

            ViewBag.Categories = await _dbContext.Categories
                .Select(c => new CategorySelectItemViewModel { Id = c.Id, Name = c.Name })
                .ToListAsync();

            return View();
        }

        [Route("/product")]
        [HttpPost]
        public async Task<IActionResult> Create([FromForm] EditProductViewModel newProductModel)
        {
            ViewBag.Discounts = await _dbContext.ProductDiscounts
                .Where(d => d.Enabled)
                .Select(d => new DiscountSelectItemViewModel { Id = d.Id, Rate = d.DiscountRate })
                .ToListAsync();

            ViewBag.Categories = await _dbContext.Categories
                .Select(c => new CategorySelectItemViewModel { Id = c.Id, Name = c.Name })
                .ToListAsync();

            if (!ModelState.IsValid)
            {
                return View(newProductModel);
            }

            // TODO: save new product...

            return View();
        }

        [Route("/product/{productId:int}/edit")]
        [HttpGet]
        public async Task<IActionResult> Edit([FromRoute] int productId)
        {
            return View();
        }

        [Route("/product/{productId:int}/edit")]
        [HttpPost]
        public async Task<IActionResult> Edit([FromRoute] int productId, [FromForm] object editProductModel)
        {
            return View();
        }

        [Route("/product/{productId:int}/delete")]
        [HttpGet]
        public async Task<IActionResult> Delete([FromRoute] int productId)
        {
            return View();
        }

        [Route("/product/{productId:int}/comment")]
        [HttpPost]
        public async Task<IActionResult> Comment([FromRoute] int productId, [FromForm] object newProductCommentModel)
        {
            // save product comment...

            return RedirectToAction(nameof(HomeController.ProductDetail), "Home", new { productId });
        }
    }
}