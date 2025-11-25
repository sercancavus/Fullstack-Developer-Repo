using App.Eticaret.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace App.Eticaret.ViewComponents
{
    public class FromTheBlogViewComponent(IHttpClientFactory httpClientFactory) : ViewComponent
    {
        public async Task<IViewComponentResult> InvokeAsync()
        {
            // TODO: Replace with DataApi endpoint when available
            return View(new List<BlogSummaryViewModel>());
        }
    }
}