using Microsoft.AspNetCore.Mvc;

namespace App.Admin.Controllers
{
    public class CategoryController : Controller
    {
        [Route("/categories")]
        [HttpGet]
        public IActionResult List()
        {
            return View();
        }

        [Route("/categories/create")]
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [Route("/categories/create")]
        [HttpPost]
        public IActionResult Create([FromForm] object newCategoryModel)
        {
            return View();
        }

        [Route("/categories/{categoryId:int}/edit")]
        [HttpGet]
        public IActionResult Edit([FromRoute] int categoryId)
        {
            return View();
        }

        [Route("/categories/{categoryId:int}/edit")]
        [HttpPost]
        public IActionResult Edit([FromRoute] int categoryId, [FromForm] object editCategoryModel)
        {
            return View();
        }

        [Route("/categories/{categoryId:int}/delete")]
        [HttpGet]
        public IActionResult Delete([FromRoute] int categoryId)
        {
            // delete actions
            return RedirectToAction("Index", "Home");
        }
    }
}