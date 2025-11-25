using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace App.Admin.Controllers
{
    [Route("/comment")]
    [Authorize(Roles = "admin")]
    public class CommentController : Controller
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