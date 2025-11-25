using App.Admin.Models.ViewModels;
using App.Services.Abstract;
using Ardalis.Result;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace App.Admin.Controllers
{
    [Authorize(Roles = "admin")]
    public class UserController : Controller
    {
        private readonly IUserService _userService;
        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [Route("/users")]
        [HttpGet]
        public async Task<IActionResult> List()
        {
            var jwt = Request.Cookies["AuthToken"] ?? string.Empty;
            var result = await _userService.ListUsersAsync(jwt);
            if (result.Status == ResultStatus.Unauthorized)
                return Unauthorized();
            if (!result.IsSuccess)
                return BadRequest(result.Errors);
            var vm = result.Value?.Select(u => new UserListItemViewModel
            {
                Id = u.Id,
                FirstName = u.FirstName,
                LastName = u.LastName,
                Email = u.Email,
                Role = u.Role,
                Enabled = u.Enabled,
                HasSellerRequest = u.HasSellerRequest
            }).ToList() ?? new List<UserListItemViewModel>();
            return View(vm);
        }

        [Route("/users/{id:int}/approve")]
        [HttpGet]
        public async Task<IActionResult> ApproveSellerRequest([FromRoute] int id)
        {
            var jwt = Request.Cookies["AuthToken"] ?? string.Empty;
            var result = await _userService.ApproveSellerRequestAsync(jwt, id);
            if (result.Status == ResultStatus.NotFound)
                return NotFound();
            if (result.Status == ResultStatus.Unauthorized)
                return Unauthorized();
            if (!result.IsSuccess)
                return BadRequest(result.Errors);
            return RedirectToAction(nameof(List));
        }

        [Route("/users/{id:int}/enable")]
        public async Task<IActionResult> Enable([FromRoute] int id)
        {
            var jwt = Request.Cookies["AuthToken"] ?? string.Empty;
            var result = await _userService.EnableUserAsync(jwt, id);
            if (result.Status == ResultStatus.NotFound)
                return NotFound();
            if (result.Status == ResultStatus.Unauthorized)
                return Unauthorized();
            if (!result.IsSuccess)
                return BadRequest(result.Errors);
            return RedirectToAction(nameof(List));
        }

        [Route("/users/{id:int}/disable")]
        public async Task<IActionResult> Disable([FromRoute] int id)
        {
            var jwt = Request.Cookies["AuthToken"] ?? string.Empty;
            var result = await _userService.DisableUserAsync(jwt, id);
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