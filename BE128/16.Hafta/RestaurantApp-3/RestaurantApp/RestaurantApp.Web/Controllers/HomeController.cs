using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using RestaurantApp.Common.Models;
using RestaurantApp.Services.Interfaces;
using RestaurantApp.Services.Repositories;
using RestaurantApp.Web.Models;
using System.Diagnostics;

namespace RestaurantApp.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly IProductRepository _productRepository;
        private readonly ICategoryRepository _categoryRepository;
        private readonly ISaleRepository _saleRepository;
        private const string CartSessionKey = "Cart";

        public HomeController(IProductRepository productRepository, ICategoryRepository categoryRepository, ISaleRepository saleRepository)
        {
            _productRepository = productRepository;
            _categoryRepository = categoryRepository;
            _saleRepository = saleRepository;
        }

        public async Task<IActionResult> Index()
        {
            var products = await _productRepository.GetAllProductsAsync();
            var categories = await _categoryRepository.GetAllCategoriesAsync();

            var viewModel = new HomeViewModel
            {
                Products = products,
                Categories = categories
            };

            return View(viewModel);
        }

        [HttpGet]
        public IActionResult GetCartCount()
        {
            var cart = GetCart();
            int count = cart.Sum(p => p.Quantity);

            return Json(new { count });
        }

        [HttpPost]
        public async Task<IActionResult> AddToCart(int productId)
        {
            var product = await _productRepository.GetProductByIdAsync(productId);

            if (product == null)
            {
                return NotFound();
            }

            var cart = GetCart();

            var existingProduct = cart.FirstOrDefault(p => p.ProductId == productId);

            if (existingProduct != null)
            {
                existingProduct.Quantity++;
                existingProduct.ProductTotalPrice += product.Price;
            }
            else
            {
                cart.Add(new SaleProducts
                {
                    ProductId = product.Id,
                    Product = product,
                    Quantity = 1,
                    ProductPrice = product.Price,
                    ProductTotalPrice = product.Price
                });
            }

            SaveCart(cart);

            return Ok();
        }

        public List<SaleProducts> GetCart()
        {
            var cart = HttpContext.Session.GetString(CartSessionKey);

            return string.IsNullOrEmpty(cart) ? new List<SaleProducts>() : JsonConvert.DeserializeObject<List<SaleProducts>>(cart);
        }

        public void SaveCart(List<SaleProducts> cart)
        {
            HttpContext.Session.SetString(CartSessionKey, JsonConvert.SerializeObject(cart));
        }

        public void ClearCart()
        {
            HttpContext.Session.Remove(CartSessionKey);
        }
    }
}
