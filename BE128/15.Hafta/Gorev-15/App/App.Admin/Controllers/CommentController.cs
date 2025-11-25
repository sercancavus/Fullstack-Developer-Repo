using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http.Json;

namespace App.Admin.Controllers
{
    [Route("/comment")]
    [Authorize(Roles = "admin")]
    public class CommentController(IHttpClientFactory httpClientFactory) : Controller
    {
        public record PendingComment(int Id, string Text, byte StarCount, string ProductName, string UserName);

        [Route("")]
        [HttpGet]
        public async Task<IActionResult> List()
        {
            var client = httpClientFactory.CreateClient("DataApi");
            var list = await client.GetFromJsonAsync<List<PendingComment>>("/api/admin/comments/pending");
            return View(list ?? new List<PendingComment>());
        }

        [Route("{commentId:int}/approve")]
        [HttpGet]
        public async Task<IActionResult> Approve([FromRoute] int commentId)
        {
            var client = httpClientFactory.CreateClient("DataApi");
            var response = await client.PostAsync($"/api/admin/comments/{commentId}/approve", null);
            if (!response.IsSuccessStatusCode) return BadRequest();
            return RedirectToAction(nameof(List));
        }
    }
}