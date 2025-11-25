using Microsoft.AspNetCore.Mvc;

namespace App.Api.File.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class FileController() : ControllerBase
    {
        [HttpPost]
        public async Task<IActionResult> Upload(IFormFile file)
        {
            if (file is null || file.Length == 0)
            {
                return BadRequest("Dosya yüklenemedi.");
            }

            var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads", file.FileName);

            using (var stream = new FileStream(path, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            return CreatedAtAction(nameof(Download), new { fileName = file.FileName }, file);
        }

        [HttpGet]
        public IActionResult Download([FromQuery] string fileName)
        {
            var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads", fileName);

            if (!System.IO.File.Exists(path))
            {
                return NotFound();
            }

            var stream = System.IO.File.OpenRead(path);
            return File(stream, "application/octet-stream");
        }
    }
}
