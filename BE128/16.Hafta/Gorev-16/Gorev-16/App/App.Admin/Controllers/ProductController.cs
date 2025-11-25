using App.Services.Abstract;
using Ardalis.Result;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace App.Admin.Controllers
{
    [Authorize(Roles = "admin")]
    public class ProductController : Controller
    {
        private readonly IProductService _productService;
        public ProductController(IProductService productService)
        {
            _productService = productService;
        }

        [Route("/products/")]
        [HttpGet]
        public async Task<IActionResult> List()
        {
            var jwt = Request.Cookies["AuthToken"] ?? string.Empty;
            var result = await _productService.ListProductsAsync(jwt);
            if (result.Status == ResultStatus.Unauthorized)
                return Unauthorized();
            if (!result.IsSuccess)
                return BadRequest(result.Errors);
            // TODO: DTO'dan Admin view modeline map (şimdilik temel alanlar yeterli)
            return View(result.Value ?? new List<App.Models.DTO.ProductDto>());
        }

        [Route("/products/{productId:int}/disable")]
        [HttpGet]
        public async Task<IActionResult> Disable([FromRoute] int productId)
        {
            // TODO: DisableProductAsync servis metodu eklenecek ve çağrılacak
            return RedirectToAction(nameof(List));
        }

        [Route("/products/{productId:int}/enable")]
        [HttpGet]
        public async Task<IActionResult> Enable([FromRoute] int productId)
        {
            // TODO: EnableProductAsync servis metodu eklenecek ve çağrılacak
            return RedirectToAction(nameof(List));
        }

        [Route("/products/{productId:int}/delete")]
        [HttpGet]
        public IActionResult Delete([FromRoute] int productId)
        {
            // TODO: DeleteProductAsync servis metodu eklenecek
            return View();
        }
    }
}