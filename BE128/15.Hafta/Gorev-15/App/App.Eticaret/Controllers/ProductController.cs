using App.Eticaret.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Net.Http.Json;

namespace App.Eticaret.Controllers
{
    [Route("/product")]
    public class ProductController(IHttpClientFactory httpClientFactory) : BaseController
    {
        [HttpGet("")]
        [Authorize(Roles = "seller")]
        public IActionResult Create()
        {
            return View();
        }

        private record CreatedResult(int Id);
        private record FileUploadResult(string fileName, string url);

        [HttpPost("")]
        [Authorize(Roles = "seller")]
        public async Task<IActionResult> Create([FromForm] SaveProductViewModel newProductModel)
        {
            if (!ModelState.IsValid)
            {
                return View(newProductModel);
            }

            var dataClient = httpClientFactory.CreateClient("DataApi");

            var createResponse = await dataClient.PostAsJsonAsync("/api/product", new
            {
                categoryId = newProductModel.CategoryId,
                discountId = newProductModel.DiscountId,
                name = newProductModel.Name,
                price = newProductModel.Price,
                description = newProductModel.Description,
                stockAmount = newProductModel.StockAmount
            });

            if (!createResponse.IsSuccessStatusCode)
            {
                ModelState.AddModelError(string.Empty, "Ürün oluşturulamadı.");
                return View(newProductModel);
            }

            var created = await createResponse.Content.ReadFromJsonAsync<CreatedResult>();
            if (created == null)
            {
                ModelState.AddModelError(string.Empty, "Geçersiz yanıt.");
                return View(newProductModel);
            }

            await SaveProductImages(created.Id, newProductModel.Images);

            ViewBag.SuccessMessage = "Ürün başarıyla eklendi.";
            ModelState.Clear();

            return View();
        }

        private async Task SaveProductImages(int productId, IList<IFormFile> images)
        {
            if (images == null || images.Count == 0) return;

            var fileClient = httpClientFactory.CreateClient("FileApi");
            var dataClient = httpClientFactory.CreateClient("DataApi");
            var baseAddress = fileClient.BaseAddress?.ToString()?.TrimEnd('/') ?? string.Empty;

            foreach (var image in images)
            {
                using var content = new MultipartFormDataContent();
                var streamContent = new StreamContent(image.OpenReadStream());
                streamContent.Headers.ContentType = new MediaTypeHeaderValue(image.ContentType);
                content.Add(streamContent, "file", image.FileName);

                var response = await fileClient.PostAsync("/api/file/upload", content);
                if (!response.IsSuccessStatusCode) continue;

                var body = await response.Content.ReadAsStringAsync();
                var uploadResult = JsonSerializer.Deserialize<FileUploadResult>(body, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });
                if (uploadResult == null) continue;

                var absoluteUrl = uploadResult.url.StartsWith("http", StringComparison.OrdinalIgnoreCase)
                    ? uploadResult.url
                    : $"{baseAddress}{uploadResult.url}";

                await dataClient.PostAsJsonAsync($"/api/product/{productId}/images", new { url = absoluteUrl });
            }
        }

        [HttpGet("{productId:int}/edit")]
        [Authorize(Roles = "seller")]
        public async Task<IActionResult> Edit([FromRoute] int productId)
        {
            var client = httpClientFactory.CreateClient("DataApi");
            var product = await client.GetFromJsonAsync<SaveProductViewModel>($"/api/product/{productId}/own");
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        [HttpPost("{productId:int}/edit")]
        [Authorize(Roles = "seller")]
        public async Task<IActionResult> Edit([FromRoute] int productId, [FromForm] SaveProductViewModel editProductModel)
        {
            if (!ModelState.IsValid)
            {
                return View(editProductModel);
            }

            var client = httpClientFactory.CreateClient("DataApi");

            var response = await client.PutAsJsonAsync($"/api/product/{productId}", new
            {
                categoryId = editProductModel.CategoryId,
                discountId = editProductModel.DiscountId,
                name = editProductModel.Name,
                price = editProductModel.Price,
                description = editProductModel.Description,
                stockAmount = editProductModel.StockAmount
            });

            if (!response.IsSuccessStatusCode)
            {
                ModelState.AddModelError(string.Empty, "Ürün güncellenemedi.");
                return View(editProductModel);
            }

            ViewBag.SuccessMessage = "Ürün başarıyla güncellendi.";

            return View(editProductModel);
        }

        [HttpGet("{productId:int}/delete")]
        [Authorize(Roles = "seller")]
        public async Task<IActionResult> Delete([FromRoute] int productId)
        {
            var client = httpClientFactory.CreateClient("DataApi");
            var response = await client.DeleteAsync($"/api/product/{productId}");
            if (!response.IsSuccessStatusCode)
            {
                return BadRequest();
            }

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

            var client = httpClientFactory.CreateClient("DataApi");
            var response = await client.PostAsJsonAsync($"/api/product/{productId}/comment", new
            {
                text = newProductCommentModel.Text,
                starCount = newProductCommentModel.StarCount
            });

            if (!response.IsSuccessStatusCode)
            {
                return BadRequest();
            }

            return Ok();
        }
    }
}