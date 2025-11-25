using App.Admin.Models.ViewModels;
using App.Services.Abstract;
using Ardalis.Result;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace App.Admin.Controllers
{
    [Route("/categories")]
    [Authorize(Roles = "admin")]
    public class CategoryController : Controller
    {
        private readonly ICategoryService _categoryService;
        public CategoryController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        [HttpGet]
        public async Task<IActionResult> List()
        {
            var jwt = Request.Cookies["AuthToken"] ?? string.Empty;
            var result = await _categoryService.ListCategoriesAsync(jwt);
            if (result.Status == ResultStatus.Unauthorized)
                return Unauthorized();
            if (!result.IsSuccess)
                return BadRequest(result.Errors);
            var vm = result.Value?.Select(c => new CategoryListViewModel
            {
                Id = c.Id,
                Name = c.Name,
                Color = c.Color,
                IconCssClass = c.IconCssClass
            }).ToList() ?? new List<CategoryListViewModel>();
            return View(vm);
        }

        [Route("create")]
        [HttpGet]
        public IActionResult Create() => View();

        [Route("create")]
        [HttpPost]
        public async Task<IActionResult> Create([FromForm] SaveCategoryViewModel newCategoryModel)
        {
            if (!ModelState.IsValid) return View(newCategoryModel);
            var jwt = Request.Cookies["AuthToken"] ?? string.Empty;
            var dto = new App.Models.DTO.CreateCategoryRequestDto
            {
                Name = newCategoryModel.Name,
                Color = newCategoryModel.Color,
                IconCssClass = newCategoryModel.IconCssClass ?? string.Empty
            };
            var result = await _categoryService.CreateCategoryAsync(jwt, dto);
            if (!result.IsSuccess)
            {
                ModelState.AddModelError(string.Empty, result.Errors.FirstOrDefault() ?? "Kategori oluşturulamadı.");
                return View(newCategoryModel);
            }
            ViewBag.SuccessMessage = "Kategori başarıyla oluşturuldu.";
            ModelState.Clear();
            return View();
        }

        [Route("{categoryId:int}/edit")]
        [HttpGet]
        public async Task<IActionResult> Edit([FromRoute] int categoryId)
        {
            var jwt = Request.Cookies["AuthToken"] ?? string.Empty;
            var result = await _categoryService.GetCategoryAsync(jwt, categoryId);
            if (result.Status == ResultStatus.NotFound) return NotFound();
            if (result.Status == ResultStatus.Unauthorized) return Unauthorized();
            if (!result.IsSuccess) return BadRequest(result.Errors);
            var vm = new SaveCategoryViewModel
            {
                Name = result.Value.Name,
                Color = result.Value.Color,
                IconCssClass = result.Value.IconCssClass
            };
            return View(vm);
        }

        [Route("{categoryId:int}/edit")]
        [HttpPost]
        public async Task<IActionResult> Edit([FromRoute] int categoryId, [FromForm] SaveCategoryViewModel editCategoryModel)
        {
            if (!ModelState.IsValid) return View(editCategoryModel);
            var jwt = Request.Cookies["AuthToken"] ?? string.Empty;
            var updateDto = new App.Models.DTO.UpdateCategoryRequestDto
            {
                Name = editCategoryModel.Name,
                Color = editCategoryModel.Color,
                IconCssClass = editCategoryModel.IconCssClass ?? string.Empty
            };
            var result = await _categoryService.UpdateCategoryAsync(jwt, categoryId, updateDto);
            if (!result.IsSuccess)
            {
                ModelState.AddModelError(string.Empty, result.Errors.FirstOrDefault() ?? "Kategori güncellenemedi.");
                return View(editCategoryModel);
            }
            ViewBag.SuccessMessage = "Kategori başarıyla güncellendi.";
            return View(editCategoryModel);
        }

        [Route("{categoryId:int}/delete")]
        [HttpGet]
        public async Task<IActionResult> Delete([FromRoute] int categoryId)
        {
            var jwt = Request.Cookies["AuthToken"] ?? string.Empty;
            var result = await _categoryService.DeleteCategoryAsync(jwt, categoryId);
            if (result.Status == ResultStatus.NotFound) return NotFound();
            if (result.Status == ResultStatus.Unauthorized) return Unauthorized();
            if (!result.IsSuccess) return BadRequest(result.Errors);
            return RedirectToAction(nameof(List));
        }
    }
}