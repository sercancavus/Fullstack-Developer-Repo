using Microsoft.AspNetCore.Mvc;

namespace App.Mvc.ViewComponents
{
    public class ProductListViewComponent : ViewComponent
    {
        public IViewComponentResult Invoke()
        {
            List<string> productNames = new() { "Elma", "Armut", "Muz" };

            return View(productNames);
        }
    }
}
