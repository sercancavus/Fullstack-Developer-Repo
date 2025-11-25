using System.Diagnostics;
using App.Mvc.Models;
using Microsoft.AspNetCore.Mvc;

namespace App.Mvc.Controllers
{
    public class HomeController : Controller
    {
        private static List<ProductViewModel> products = new()
        {
            new ProductViewModel {Id = 1, Name = "Urun1", Price = 10.90},
            new ProductViewModel {Id = 2, Name = "Urun2", Price = 20.90},
            new ProductViewModel {Id = 3, Name = "Urun3", Price = 30.90},
        };
        public IActionResult Index()
        {
            return View();
        }

        // 1. Yöntem : ViewModel ile

        [Route("model-ile-products")]
        public IActionResult WithViewModel()
        {
            return View(products);
        }

        // 2. Yöntem : ViewBag ile

        [Route("viewbag-ile-products")]
        public IActionResult WithViewBag()
        {
            ViewBag.Products = products;
            return View();
        }


        public void DictionaryUsage()
        {
            // Key-Value prensibi ile veri tutar.

            // 1)

            //Dictionary<string, int> data = new()
            //{
            //    {"ali",90 },
            //    {"veli",30 },
            //    {"aslý",70 }
            //};

            // 2)

            Dictionary<string, int> data = new();

            data.Add("ali", 90);
            data.Add("veli", 30);
            data.Add("aslý", 70);

            data["tufan"] = 45;

            // Eleman silme

            data.Remove("aslý");

            // içerisinde þu key var mý kontrolü;

            var IsExists = data.Keys.Contains("veli");

            // dictionary içindeki bir deðeri okuma;

            int point = data["veli"];

        }


        // 3. Yöntem : ViewData ile

        [Route("viewdata-ile-products")]
        public IActionResult WithViewData()
        {
            // 1. Yöntem
            //ViewData.Add("Products" , products);

            // 2. Yöntem
            ViewData["Products"] = products;

            return View();
        }

        [Route("viewdata-ile-redirect")]
        public IActionResult RedirectWithViewData()
        {
            ViewData["Products"] = products;

            return RedirectToAction(nameof(RedirectWithViewDataTarget));
        }


        [Route("viewdata-ile-redirect-target")]
        public IActionResult RedirectWithViewDataTarget()
        {
            return View("~/Views/Home/WithViewData.cshtml");
        }



        // 4. Yöntem : TempData ile

        [Route("tempdata-ile-products")]
        public IActionResult WithTempData()
        {
            TempData["Products"] = products;

            return View();
        }

        [Route("tempdata-ile-redirect")]
        public IActionResult RedirectWithTempData()
        {
            // 1)
            //TempData["Academy"] = "Siliconmade Academy";


            // 2) 
            TempData["Products"] = products;

            return RedirectToAction(nameof(RedirectWithTempDataTarget));
        }

        [Route("tempdata-ile-redirect-target")]
        public IActionResult RedirectWithTempDataTarget()
        {
            return View("~/Views/Home/WithTempData.cshtml");
        }


    }
}
