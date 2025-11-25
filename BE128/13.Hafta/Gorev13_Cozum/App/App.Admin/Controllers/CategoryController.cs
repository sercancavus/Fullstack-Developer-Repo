using App.Admin.Models.ViewModels;
using App.Data.Entities;
using App.Data.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace App.Admin.Controllers
{
    [Route("/categories")]
    public class CategoryController(ApplicationDbContext dbContext) : Controller
    {
        [HttpGet]
        public async Task<IActionResult> List()
        {
            var categories = await dbContext.Categories
                .Select(c => new CategoryListViewModel
                {
                    Id = c.Id,
                    Name = c.Name,
                    Color = c.Color,
                    IconCssClass = c.IconCssClass
                })
                .ToListAsync();

            return View(categories);
        }

        [Route("create")]
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [Route("create")]
        [HttpPost]
        public async Task<IActionResult> Create([FromForm] SaveCategoryViewModel newCategoryModel)
        {
            if (!ModelState.IsValid)
            {
                return View(newCategoryModel);
            }

            var categoryEntity = new CategoryEntity
            {
                Name = newCategoryModel.Name,
                Color = newCategoryModel.Color,
                IconCssClass = string.Empty,
                CreatedAt = DateTime.UtcNow
            };

            dbContext.Categories.Add(categoryEntity);
            await dbContext.SaveChangesAsync();

            ViewBag.SuccessMessage = "Kategori başarıyla oluşturuldu.";
            ModelState.Clear();

            return View();
        }

        [Route("{categoryId:int}/edit")]
        [HttpGet]
        public async Task<IActionResult> Edit([FromRoute] int categoryId)
        {
            var category = await dbContext.Categories.FindAsync(categoryId);
            if (category is null)
            {
                return NotFound();
            }

            var editCategoryModel = new SaveCategoryViewModel
            {
                Name = category.Name,
                Color = category.Color,
                IconCssClass = category.IconCssClass
            };

            return View(editCategoryModel);
        }

        [Route("{categoryId:int}/edit")]
        [HttpPost]
        public async Task<IActionResult> Edit([FromRoute] int categoryId, [FromForm] SaveCategoryViewModel editCategoryModel)
        {
            if (!ModelState.IsValid)
            {
                return View(editCategoryModel);
            }

            var category = await dbContext.Categories.FindAsync(categoryId);
            if (category is null)
            {
                return NotFound();
            }

            category.Name = editCategoryModel.Name;
            category.Color = editCategoryModel.Color;

            dbContext.Categories.Update(category);
            await dbContext.SaveChangesAsync();

            ViewBag.SuccessMessage = "Kategori başarıyla güncellendi.";
            ModelState.Clear();

            return View();
        }

        [Route("{categoryId:int}/delete")]
        [HttpGet]
        public async Task<IActionResult> Delete([FromRoute] int categoryId)
        {
            var category = await dbContext.Categories.FindAsync(categoryId);
            if (category is null)
            {
                return NotFound();
            }

            dbContext.Categories.Remove(category);
            await dbContext.SaveChangesAsync();

            return RedirectToAction(nameof(List));
        }
    }
}