using App.Admin.Models.ViewModels;
using App.Data.Entities;
using App.Data.Infrastructure;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace App.Admin.Controllers
{
    [Authorize(Roles = "admin")]
    public class UserController(DataRepository<UserEntity> userRepo) : Controller
    {
        [Route("/users")]
        [HttpGet]
        public async Task<IActionResult> List()
        {
            List<UserListItemViewModel> users = await userRepo.GetAll()
                .Where(u => u.RoleId != 1)
                .Select(u => new UserListItemViewModel
                {
                    Id = u.Id,
                    FirstName = u.FirstName,
                    LastName = u.LastName,
                    Email = u.Email,
                    Role = u.Role.Name,
                    Enabled = u.Enabled,
                    HasSellerRequest = u.HasSellerRequest
                })
                .ToListAsync();

            return View(users);
        }

        [Route("/users/{id:int}/approve")]
        [HttpGet]
        public async Task<IActionResult> ApproveSellerRequest([FromRoute] int id)
        {
            var user = await userRepo.GetByIdAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            if (!user.HasSellerRequest)
            {
                return BadRequest();
            }

            user.HasSellerRequest = false;
            user.RoleId = 2; // seller

            await userRepo.UpdateAsync(user);

            return RedirectToAction(nameof(List));
        }

        [Route("/users/{id:int}/enable")]
        public async Task<IActionResult> Enable([FromRoute] int id)
        {
            var user = await userRepo.GetByIdAsync(id);

            if (user == null)
            {
                return NotFound();
            }

            user.Enabled = true;
            await userRepo.UpdateAsync(user);

            return RedirectToAction(nameof(List));
        }

        [Route("/users/{id:int}/disable")]
        public async Task<IActionResult> Disable([FromRoute] int id)
        {
            var user = await userRepo.GetByIdAsync(id);

            if (user == null)
            {
                return NotFound();
            }

            user.Enabled = false;
            await userRepo.UpdateAsync(user);

            return RedirectToAction(nameof(List));
        }
    }
}