using App.Data.Entities;
using App.Data.Infrastructure;
using App.Eticaret.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace App.Eticaret.Controllers
{
    [Route("/product")]
    public class ProductController(DataRepository<ProductEntity> prRepo,
        DataRepository<ProductImageEntity> priRepo,
        DataRepository<ProductCommentEntity> prcRepo) : BaseController
    {
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

            var productEntity = new ProductEntity
            {
                SellerId = 2, // TODO: User'ı al
                CategoryId = newProductModel.CategoryId,
                DiscountId = newProductModel.DiscountId,
                Name = newProductModel.Name,
                Price = newProductModel.Price,
                Description = newProductModel.Description,
                StockAmount = newProductModel.StockAmount,
            };

            productEntity = await prRepo.AddAsync(productEntity);

            await SaveProductImages(productEntity.Id, newProductModel.Images);

            ViewBag.SuccessMessage = "Ürün başarıyla eklendi.";
            ModelState.Clear();

            return View();
        }

        private async Task SaveProductImages(int productId, IList<IFormFile> images)
        {
            foreach (var image in images)
            {
                var productImageEntity = new ProductImageEntity
                {
                    ProductId = productId,
                    Url = $"/uploads/{Guid.NewGuid()}{Path.GetExtension(image.FileName)}"
                };

                productImageEntity = await priRepo.AddAsync(productImageEntity);

                await using var fileStream = new FileStream($"wwwroot{productImageEntity.Url}", FileMode.Create);
                await image.CopyToAsync(fileStream);
            }
        }

        [HttpGet("{productId:int}/edit")]
        [Authorize(Roles = "seller")]
        public async Task<IActionResult> Edit([FromRoute] int productId)
        {
            var productEntity = await prRepo.GetByIdAsync(productId);
            if (productEntity is null)
            {
                return NotFound();
            }

            if (productEntity.SellerId != GetUserId())
            {
                return Unauthorized();
            }

            var viewModel = new SaveProductViewModel
            {
                CategoryId = productEntity.CategoryId,
                DiscountId = productEntity.DiscountId,
                Name = productEntity.Name,
                Price = productEntity.Price,
                Description = productEntity.Description,
                StockAmount = productEntity.StockAmount
            };

            return View(viewModel);
        }

        [HttpPost("{productId:int}/edit")]
        [Authorize(Roles = "seller")]
        public async Task<IActionResult> Edit([FromRoute] int productId, [FromForm] SaveProductViewModel editProductModel)
        {
            if (!ModelState.IsValid)
            {
                return View(editProductModel);
            }

            var productEntity = await prRepo.GetByIdAsync(productId);

            if (productEntity is null)
            {
                return NotFound();
            }

            if (productEntity.SellerId != GetUserId())
            {
                return Unauthorized();
            }

            productEntity.CategoryId = editProductModel.CategoryId;
            productEntity.DiscountId = editProductModel.DiscountId;
            productEntity.Name = editProductModel.Name;
            productEntity.Price = editProductModel.Price;
            productEntity.Description = editProductModel.Description;
            productEntity.StockAmount = editProductModel.StockAmount;

            await prRepo.UpdateAsync(productEntity);

            ViewBag.SuccessMessage = "Ürün başarıyla güncellendi.";

            return View(editProductModel);
        }

        [HttpGet("{productId:int}/delete")]
        [Authorize(Roles = "seller")]
        public async Task<IActionResult> Delete([FromRoute] int productId)
        {
            var userId = GetUserId();

            if (userId == null)
            {
                return Unauthorized();
            }

            if (!await prRepo.GetAll().AnyAsync(x => x.Id == productId && x.SellerId == userId))
            {
                return NotFound();
            }

            await prRepo.DeleteAsync(productId);

            ViewBag.SuccessMessage = "Ürün başarıyla silindi.";

            return View();
        }

        [HttpPost("{productId:int}/comment")]
        [Authorize(Roles = "buyer, seller")]
        public async Task<IActionResult> Comment([FromRoute] int productId, [FromForm] SaveProductCommentViewModel newProductCommentModel)
        {
            var userId = GetUserId();

            if (userId == null)
            {
                return Unauthorized();
            }

            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            if (!await prRepo.GetAll().AnyAsync(x => x.Id == productId))
            {
                return NotFound();
            }

            if (await prcRepo.GetAll().AnyAsync(x => x.ProductId == productId && x.UserId == userId))
            {
                return BadRequest();
            }

            var productCommentEntity = new ProductCommentEntity
            {
                ProductId = productId,
                UserId = userId.Value,
                Text = newProductCommentModel.Text,
                StarCount = newProductCommentModel.StarCount,
            };

            await prcRepo.AddAsync(productCommentEntity);

            return Ok();
        }
    }
}