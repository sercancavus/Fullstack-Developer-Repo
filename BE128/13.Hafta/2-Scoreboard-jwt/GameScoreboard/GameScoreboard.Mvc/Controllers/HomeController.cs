using System.Diagnostics;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Threading.Tasks;
using GameScoreboard.Data.Entities;
using GameScoreboard.Mvc.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GameScoreboard.Mvc.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly DbContext _dbContext;

        public HomeController(ILogger<HomeController> logger, DbContext dbContext)
        {
            _logger = logger;
            _dbContext = dbContext;
        }

        public async Task<IActionResult> Index()
        {
            var scores = await _dbContext.Set<HighScoreEntity>()
                .Include(s => s.User)
                .OrderByDescending(s => s.Score)
                .Take(10)
                .Select(s => new HighScoreViewModel
                {
                    Nickname = s.User.Nickname,
                    Score = s.Score
                })
                .ToListAsync();

            // Select metodu sorgu yapýlýrken, sorgulanan veride bir dönüþüm yapmak için kullanýlýr.
            // Sorgulanan veri HighScoreViewModel'in içine sadece istenen veriler(Nickname, Score) oalcak þekilde yerleþtirildi.

            return View(scores);
        }

        [HttpGet]
        public IActionResult NewScore()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> NewScore([FromForm] NewScoreViewModel newScoreViewModel)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            //var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier) ?? throw new Exception() );


            var userId = int.Parse(User.FindFirstValue(JwtRegisteredClaimNames.Sub) ?? throw new Exception() );

            await AddOrUpdateScore(userId, newScoreViewModel.Score);

            return RedirectToAction("Index");
        }
        
        private async Task AddOrUpdateScore(int userId, int score)
        {
            var userRecord = await _dbContext.Set<HighScoreEntity>().FirstOrDefaultAsync(u => u.UserId == userId);

            if (userRecord is null)
            {
                userRecord = new HighScoreEntity
                {
                    UserId = userId,
                    Score = score
                };

                _dbContext.Add(userRecord);
            }
            else
            {
                userRecord.Score = Math.Max(userRecord.Score, score);
            }

            await _dbContext.SaveChangesAsync();
        }
    }
}
