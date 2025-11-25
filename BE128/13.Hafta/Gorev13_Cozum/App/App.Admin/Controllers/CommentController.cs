using App.Data.Infrastructure;
using Microsoft.AspNetCore.Mvc;

namespace App.Admin.Controllers
{
    [Route("/comment")]
    public class CommentController(ApplicationDbContext dbContext) : Controller
    {
        [Route("")]
        [HttpGet]
        public IActionResult List()
        {

            return View();
        }

        [Route("{commentId:int}/approve")]
        [HttpGet]
        public IActionResult Approve([FromRoute] int commentId)
        {
            return RedirectToAction(nameof(List));
        }
    }
}