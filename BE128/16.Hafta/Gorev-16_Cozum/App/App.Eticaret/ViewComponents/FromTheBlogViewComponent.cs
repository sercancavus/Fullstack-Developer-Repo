using App.Eticaret.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace App.Eticaret.ViewComponents
{
    public class FromTheBlogViewComponent(IHttpClientFactory clientFactory) : ViewComponent
    {
        private HttpClient Client => clientFactory.CreateClient("Api.Data");

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var response = await Client.GetAsync("blogs?take=3");

            if (!response.IsSuccessStatusCode)
            {
                return Content("Veri alınamadı.");
            }

            var model = await response.Content.ReadFromJsonAsync<List<BlogSummaryViewModel>>();

            return View(model);

            //var viewModel = await repo.GetAll()
            //        .Where(e => e.Enabled)
            //        .OrderByDescending(e => e.CreatedAt)
            //        .Take(3)
            //        .Select(e => new BlogSummaryViewModel
            //        {
            //            Id = e.Id,
            //            Title = e.Title,
            //            SummaryContent = e.Content.Substring(0, 100),
            //            ImageUrl = e.ImageUrl,
            //            CommentCount = e.Comments.Count,
            //            CreatedAt = e.CreatedAt
            //        })
            //        .ToListAsync();

            //return View(viewModel);
        }
    }
}