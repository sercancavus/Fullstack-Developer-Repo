using App.Eticaret.Models.ViewModels;
using App.Services.Abstract;
using Ardalis.Result;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace App.Eticaret.Controllers
{
    [Authorize(Roles = "seller, buyer")]
    public class ProfileController : BaseController
    {
        private readonly IProfileService _profileService;
        private readonly IOrderService _orderService;
        public ProfileController(IProfileService profileService, IOrderService orderService)
        {
            _profileService = profileService;
            _orderService = orderService;
        }

        [HttpGet("/profile")]
        public async Task<IActionResult> Details()
        {
            var jwt = Request.Cookies["AuthToken"] ?? string.Empty;
            var result = await _profileService.GetProfileAsync(jwt);
            if (result.Status == ResultStatus.Unauthorized)
                return RedirectToAction("Login", "Auth");
            if (!result.IsSuccess)
                return BadRequest(result.Errors);
            string? previousSuccessMessage = TempData["SuccessMessage"]?.ToString();
            if (previousSuccessMessage is not null)
            {
                SetSuccessMessage(previousSuccessMessage);
            }
            // TODO: DTO'dan ViewModel'e map
            return View(result.Value);
        }

        [HttpPost("/profile")]
        public async Task<IActionResult> Edit([FromForm] ProfileDetailsViewModel editMyProfileModel)
        {
            if (!ModelState.IsValid)
            {
                return View(editMyProfileModel);
            }
            var jwt = Request.Cookies["AuthToken"] ?? string.Empty;
            var dto = new App.Models.DTO.UpdateProfileRequestDto
            {
                FirstName = editMyProfileModel.FirstName,
                LastName = editMyProfileModel.LastName,
                Password = !string.IsNullOrWhiteSpace(editMyProfileModel.Password) && editMyProfileModel.Password != "******"
                    ? editMyProfileModel.Password
                    : null
            };
            var result = await _profileService.UpdateProfileAsync(jwt, dto);
            if (!result.IsSuccess)
            {
                ModelState.AddModelError(string.Empty, result.Errors.FirstOrDefault() ?? "Profil güncellenemedi.");
                return View(editMyProfileModel);
            }
            TempData["SuccessMessage"] = "Profiliniz başarıyla güncellendi.";
            return RedirectToAction(nameof(Details));
        }

        [HttpGet("/my-orders")]
        public async Task<IActionResult> MyOrders()
        {
            var jwt = Request.Cookies["AuthToken"] ?? string.Empty;
            var result = await _orderService.GetMyOrdersAsync(jwt);
            if (result.Status == ResultStatus.Unauthorized)
                return Unauthorized();
            if (!result.IsSuccess)
                return BadRequest(result.Errors);
            // TODO: DTO'dan ViewModel'e map
            return View(result.Value?.Orders ?? new List<App.Models.DTO.MyOrderSummaryDto>());
        }

        // MyProducts action için ileride IProductService ile refaktör yapılabilir.
    }
}