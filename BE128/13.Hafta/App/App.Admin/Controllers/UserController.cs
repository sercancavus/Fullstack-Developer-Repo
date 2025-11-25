using App.Admin.Models.ViewModels;
using App.Data.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace App.Admin.Controllers
{
    public class UserController(ApplicationDbContext dbContext) : Controller
    {
        [Route("/users")]
        [HttpGet]
        public async Task<IActionResult> List()
        {
            List<UserListItemViewModel> users = await dbContext.Users
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
            var user = await dbContext.Users.FindAsync(id);
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
            await dbContext.SaveChangesAsync();


            return RedirectToAction(nameof(List));
        }

        [Route("/users/{id:int}/enable")]
        public async Task<IActionResult> Enable([FromRoute] int id)
        {
            var user = await dbContext.Users.FindAsync(id);

            if (user == null)
            {
                return NotFound();
            }

            user.Enabled = true;
            await dbContext.SaveChangesAsync();

            return RedirectToAction(nameof(List));
        }


        [Route("/users/{id:int}/disable")]
        public async Task<IActionResult> Disable([FromRoute] int id)
        {
            var user = await dbContext.Users.FindAsync(id);

            if (user == null)
            {
                return NotFound();
            }

            user.Enabled = false;
            await dbContext.SaveChangesAsync();

            return RedirectToAction(nameof(List));
        }
    }
}