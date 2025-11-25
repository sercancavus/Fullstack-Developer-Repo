using App.Mvc.Models;
using Microsoft.AspNetCore.Mvc;

namespace App.Mvc.ViewComponents
{
    public class GreetingViewComponent : ViewComponent
    {
        public IViewComponentResult Invoke(string isim)
        {
            return View(  new GreetingViewModel { Name = isim }  );
        }
    }
}
