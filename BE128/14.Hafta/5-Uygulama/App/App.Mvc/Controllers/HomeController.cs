using System.Diagnostics;
using System.Threading.Tasks;
using App.Business.Services;
using App.Mvc.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace App.Mvc.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly UserService _userService;

        public HomeController(ILogger<HomeController> logger, UserService userService)
        {
            _logger = logger;
            _userService = userService;
        }

        public async Task<IActionResult> Index()
        {
            //var users = await _dbContext.Users.ToListAsync();
            //return View(users);

            // UserService'deki GetUsers() metodunu kullan ve kullanýcýlarý çaðýr!


            var users = await _userService.GetUsers();

            var viewModel = users.Select(u => new UserViewModel
            {
                Id = u.Id,
                Name = u.Name
            }).ToList();

            return View(viewModel);

        }

    }
}
