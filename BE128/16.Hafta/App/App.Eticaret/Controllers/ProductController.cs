using App.Eticaret.Models.ViewModels;
using App.Models.DTO.Product;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace App.Eticaret.Controllers
{
    [Route("/product")]
    public class ProductController(IHttpClientFactory clientFactory) : BaseController
    {
        private HttpClient Client => clientFactory.CreateClient("Api.Data");

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

            var response = await Client.PostAsJsonAsync("/product", newProductModel);

            if (!response.IsSuccessStatusCode)
            {
                ViewBag.ErrorMessage = "An error occurred while creating the product. Please try again later.";
                return View(newProductModel);
            }

            SetSuccessMessage("Ürün başarıyla eklendi.");
            ModelState.Clear();

            return View();
        }

        [HttpGet("{productId:int}/edit")]
        [Authorize(Roles = "seller")]
        public async Task<IActionResult> Edit([FromRoute] int productId)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            var response = await Client.GetAsync($"/product/{productId}");

            if (!response.IsSuccessStatusCode)
            {
                return NotFound();
            }

            var product = await response.Content.ReadFromJsonAsync<ProductGetResult>();

            if (product is null)
            {
                return NotFound();
            }

            if (product.SellerId != GetUserId())
            {
                return Forbid();
            }

            var viewModel = new SaveProductViewModel
            {
                CategoryId = product.CategoryId,
                DiscountId = product.DiscountId,
                Name = product.Name,
                Price = product.Price,
                Description = product.Description,
                StockAmount = product.StockAmount
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

            var response = await Client.GetAsync($"/product/{productId}");

            if (!response.IsSuccessStatusCode)
            {
                return NotFound();
            }

            var product = await response.Content.ReadFromJsonAsync<ProductGetResult>();

            if (product is null)
            {
                return NotFound();
            }

            if (product.SellerId != GetUserId())
            {
                return Forbid();
            }

            product.CategoryId = editProductModel.CategoryId;
            product.DiscountId = editProductModel.DiscountId;
            product.Name = editProductModel.Name;
            product.Price = editProductModel.Price;
            product.Description = editProductModel.Description;
            product.StockAmount = editProductModel.StockAmount;

            response = await Client.PutAsJsonAsync($"/product/{productId}", product);

            if (!response.IsSuccessStatusCode)
            {
                ViewBag.ErrorMessage = "An error occurred while updating the product. Please try again later.";
                return View(editProductModel);
            }

            ViewBag.SuccessMessage = "Ürün başarıyla güncellendi.";
            return View(editProductModel);
        }

        [HttpGet("{productId:int}/delete")]
        [Authorize(Roles = "seller")]
        public async Task<IActionResult> Delete([FromRoute] int productId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var userId = GetUserId();

            if (userId == null)
            {
                return Unauthorized();
            }

            var response = await Client.GetAsync($"/product/{productId}");

            if (!response.IsSuccessStatusCode)
            {
                return NotFound();
            }

            var product = await response.Content.ReadFromJsonAsync<ProductGetResult>();

            if (product is null)
            {
                return NotFound();
            }

            if (product.SellerId != userId)
            {
                return Forbid();
            }

            response = await Client.DeleteAsync($"/product/{productId}");

            if (!response.IsSuccessStatusCode)
            {
                ViewBag.ErrorMessage = "An error occurred while deleting the product. Please try again later.";
                return View();
            }

            SetSuccessMessage("Ürün başarıyla silindi.");
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

            var response = await Client.PostAsJsonAsync($"/products/{productId}/comment", newProductCommentModel);

            if (!response.IsSuccessStatusCode)
            {
                return NotFound();
            }

            return Ok();
        }
    }
}