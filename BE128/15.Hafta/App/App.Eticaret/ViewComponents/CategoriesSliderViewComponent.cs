using App.Eticaret.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace App.Eticaret.ViewComponents
{
    public class CategoriesSliderViewComponent(IHttpClientFactory clientFactory) : ViewComponent
    {
        private HttpClient Client => clientFactory.CreateClient("Api.Data");

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var response = await Client.GetAsync("categories/slider");

            if (!response.IsSuccessStatusCode)
            {
                return Content("Veri alınamadı.");
            }

            var model = await response.Content.ReadFromJsonAsync<List<CategorySliderViewModel>>();

            return View(model);
        }
    }
}