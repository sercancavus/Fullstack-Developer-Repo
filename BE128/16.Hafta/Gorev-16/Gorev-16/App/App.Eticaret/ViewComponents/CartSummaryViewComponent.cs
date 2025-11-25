using Microsoft.AspNetCore.Mvc;
using System.Net.Http.Json;

namespace App.Eticaret.ViewComponents;

public class CartSummaryViewComponent(IHttpClientFactory httpClientFactory) : ViewComponent
{
    public class SummaryModel
    {
        public int Count { get; set; }
        public decimal Total { get; set; }
    }

    public async Task<IViewComponentResult> InvokeAsync()
    {
        try
        {
            var client = httpClientFactory.CreateClient("DataApi");
            var summary = await client.GetFromJsonAsync<SummaryModel>("/api/cart/summary");
            return View(summary ?? new SummaryModel());
        }
        catch
        {
            return View(new SummaryModel());
        }
    }
}
