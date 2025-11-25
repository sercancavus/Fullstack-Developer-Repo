using Microsoft.AspNetCore.Authorization;
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

        [HttpGet]
        public async Task<IActionResult> GetProductPrice(int productId)
        {
            var product = await _productRepository.GetProductByIdAsync(productId);

            if (product == null)
            {
                return NotFound();
            }

            return Json(new { price = product.Price });
        }

        [HttpPost]
        public IActionResult RemoveFromCart(int productId)
        {
            var cart = GetCart();
            var existingProduct = cart.FirstOrDefault(p => p.ProductId == productId);

            if (existingProduct != null)
            {
                if (existingProduct.Quantity > 1)
                {
                    existingProduct.Quantity--;
                    existingProduct.ProductTotalPrice -= existingProduct.ProductPrice;
                }
                else
                {
                    cart.Remove(existingProduct);
                }
            }

            return Ok();
        }
        [Authorize]
        [HttpPost("SaveOrder/{userId}")]
        public async Task<IActionResult> SaveOrder([FromBody] List<SaleProducts> cart, [FromRoute] string userId)
        {
            try
            {
                if (cart == null || cart.Count == 0)
                {
                    return BadRequest(new { message = "Cart is empty" });
                }

                var sale = new Sale
                {
                    SaleDate = DateTime.Now,
                    TotalPrice = 0
                };

                await _saleRepository.AddSaleAsync(sale);

                var saleId = sale.Id;
                decimal totalPrice = 0;

                foreach (var item in cart)
                {
                    var product = await _productRepository.GetProductByIdAsync(item.ProductId);

                    item.ProductPrice = product.Price;
                    item.ProductTotalPrice = item.ProductPrice * item.Quantity;
                    totalPrice += item.ProductTotalPrice;
                    item.SaleId = saleId;

                    if (!User.IsInRole("Admin"))
                    {
                        item.UserId = userId;
                    }

                    await _saleRepository.AddSaleProductAsync(item);
                }

                sale.TotalPrice = totalPrice;
                await _saleRepository.UpdateSaleAsync(sale);

                return Ok(new { success = true });
            }
            catch (Exception ex)
            {
                return BadRequest(new {message = $"Error: {ex.Message}", datails = ex.InnerException?.Message});
            }

        }

        [HttpGet]
        public async Task<IActionResult> GetTodaySales()
        {
            var today = DateTime.Today;
            var sales = await _saleRepository.GetSalesByDateAsync(today);

            var salesObj = sales.Select(s => new
            {
                s.Id,
                s.SaleDate,
                s.TotalPrice,
                Products = s.SaleProducts.Select(sp => new
                {
                    sp.ProductId,
                    sp.Product.Name,
                    sp.Product.Price,
                    sp.Quantity,
                    sp.ProductTotalPrice
                })
            });

            return Json(salesObj);
        }

        [HttpGet("GetTodaySales/{userId}")]
        public async Task<IActionResult> GetTodaySales([FromRoute] string userId)
        {
            var today = DateTime.Today;
            var sales = await _saleRepository.GetSalesByDateAsync(today, userId);

            var salesObj = sales.Select(s => new
            {
                s.Id,
                s.SaleDate,
                s.TotalPrice,
                Products = s.SaleProducts.Select(sp => new
                {
                    sp.ProductId,
                    sp.Product.Name,
                    sp.Product.Price,
                    sp.Quantity,
                    sp.ProductTotalPrice
                })
            });

            return Json(salesObj);
        }

        [HttpPost]
        public async Task<IActionResult> EditOrder([FromBody] EditOrderModel model)
        {
            if (model == null || model.OrderId <=0)
            {
                return BadRequest(new { success = false, message = "Invalid Data" });
            }

            var sale = await _saleRepository.GetSaleByIdAsync(model.OrderId);

            if (sale == null)
            {
                return NotFound(new { success = false , message = "Order not found"});
            }

            foreach (var item in model.Products)
            {
                var saleProduct = sale.SaleProducts.FirstOrDefault(p => p.ProductId == item.ProductId);

                if (saleProduct != null)
                {
                    saleProduct.Quantity = item.Quantity;
                    saleProduct.ProductTotalPrice = saleProduct.ProductPrice * saleProduct.Quantity;
                }
            }

            sale.TotalPrice = sale.SaleProducts.Sum(p => p.ProductTotalPrice);

            await _saleRepository.UpdateSaleAsync(sale);

            return Ok(new { success =true});

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
