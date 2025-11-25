using App.Services.Abstract;
using Ardalis.Result;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace App.Admin.Controllers
{
    [Route("/comment")]
    [Authorize(Roles = "admin")]
    public class CommentController : Controller
    {
        private readonly IProductCommentService _commentService;
        public CommentController(IProductCommentService commentService)
        {
            _commentService = commentService;
        }

        [Route("")]
        [HttpGet]
        public async Task<IActionResult> List()
        {
            var jwt = Request.Cookies["AuthToken"] ?? string.Empty;
            var result = await _commentService.ListPendingCommentsAsync(jwt);
            if (result.Status == ResultStatus.Unauthorized)
                return Unauthorized();
            if (!result.IsSuccess)
                return BadRequest(result.Errors);
            return View(result.Value ?? new List<App.Models.DTO.ProductCommentDto>());
        }

        [Route("{commentId:int}/approve")]
        [HttpGet]
        public async Task<IActionResult> Approve([FromRoute] int commentId)
        {
            var jwt = Request.Cookies["AuthToken"] ?? string.Empty;
            var result = await _commentService.ApproveCommentAsync(jwt, commentId);
            if (result.Status == ResultStatus.NotFound)
                return NotFound();
            if (result.Status == ResultStatus.Unauthorized)
                return Unauthorized();
            if (!result.IsSuccess)
                return BadRequest(result.Errors);
            return RedirectToAction(nameof(List));
        }
    }
}