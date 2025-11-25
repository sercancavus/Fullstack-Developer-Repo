using App.Eticaret.Models.ViewModels;
using App.Services.Abstract;
using Ardalis.Result;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace App.Eticaret.Controllers
{
    [Route("/product")]
    public class ProductController : BaseController
    {
        private readonly IProductService _productService;
        public ProductController(IProductService productService)
        {
            _productService = productService;
        }

        [HttpGet("")]
        [Authorize(Roles = "seller")]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost("")]
        [Authorize(Roles = "seller")]
        public async Task<IActionResult> Create([FromForm] SaveProductViewModel newProductModel)
        {
            if (!ModelState.IsValid)
            {
                return View(newProductModel);
            }
            var jwt = Request.Cookies["AuthToken"] ?? string.Empty;
            var dto = new App.Models.DTO.CreateProductRequestDto
            {
                CategoryId = newProductModel.CategoryId,
                DiscountId = newProductModel.DiscountId,
                Name = newProductModel.Name,
                Price = newProductModel.Price,
                Description = newProductModel.Description,
                StockAmount = newProductModel.StockAmount
            };
            var result = await _productService.CreateProductAsync(jwt, dto);
            if (!result.IsSuccess)
            {
                ModelState.AddModelError(string.Empty, result.Errors.FirstOrDefault() ?? "Ürün oluşturulamadı.");
                return View(newProductModel);
            }
            // TODO: Görsel ekleme servis ile yapılacak
            ViewBag.SuccessMessage = "Ürün başarıyla eklendi.";
            ModelState.Clear();
            return View();
        }

        [HttpGet("{productId:int}/edit")]
        [Authorize(Roles = "seller")]
        public async Task<IActionResult> Edit([FromRoute] int productId)
        {
            var jwt = Request.Cookies["AuthToken"] ?? string.Empty;
            var result = await _productService.GetProductDetailAsync(jwt, productId);
            if (result.Status == ResultStatus.NotFound)
                return NotFound();
            if (result.Status == ResultStatus.Unauthorized)
                return Unauthorized();
            if (!result.IsSuccess)
                return BadRequest(result.Errors);
            // TODO: DTO'dan ViewModel'e map
            return View(result.Value);
        }

        [HttpPost("{productId:int}/edit")]
        [Authorize(Roles = "seller")]
        public async Task<IActionResult> Edit([FromRoute] int productId, [FromForm] SaveProductViewModel editProductModel)
        {
            if (!ModelState.IsValid)
            {
                return View(editProductModel);
            }
            // TODO: UpdateProductAsync ile refaktör
            ViewBag.SuccessMessage = "Ürün başarıyla güncellendi.";
            return View(editProductModel);
        }

        [HttpGet("{productId:int}/delete")]
        [Authorize(Roles = "seller")]
        public async Task<IActionResult> Delete([FromRoute] int productId)
        {
            // TODO: DeleteProductAsync ile refaktör
            ViewBag.SuccessMessage = "Ürün başarıyla silindi.";
            return View();
        }

        [HttpPost("{productId:int}/comment")]
        [Authorize(Roles = "buyer, seller")]
        public async Task<IActionResult> Comment([FromRoute] int productId, [FromForm] SaveProductCommentViewModel newProductCommentModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            // TODO: AddCommentAsync ile refaktör
            return Ok();
        }
    }
}