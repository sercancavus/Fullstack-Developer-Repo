using Microsoft.AspNetCore.Mvc;

namespace Admin.Mvc.Controllers
{
    public class CategoryController : Controller
    {
        public IActionResult Create()
        {
            return View();
        }
        [Route("category/{id}/edit")]
        public IActionResult Edit(int id)
        {
            // Kategori düzenleme için id parametresi alýnýr
            return View();
        }
        public IActionResult Delete()
        {
            return View();
        }
    }
}