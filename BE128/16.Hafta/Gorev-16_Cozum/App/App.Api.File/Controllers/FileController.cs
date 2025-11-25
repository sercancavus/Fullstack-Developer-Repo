using App.Models.DTO.File;
using App.Services.Abstract;
using Ardalis.Result.AspNetCore;
using Microsoft.AspNetCore.Mvc;

namespace App.Api.File.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class FileController(IFileService fileService) : ControllerBase
    {
        [HttpPost]
        public async Task<IActionResult> Upload(IFormFile file)
        {
            if (file is null || file.Length == 0)
            {
                return BadRequest("Dosya yüklenemedi.");
            }

            var result = await fileService.UploadAsync(new FileUploadRequest
            {
                Stream = file.OpenReadStream(),
                Name = file.FileName
            });

            if (!result.IsSuccess)
            {
                return result.ToActionResult(this).Result ?? BadRequest("Dosya yüklenemedi.");
            }

            return CreatedAtAction(nameof(Download), new { fileName = result.Value.FileName }, result.Value);
        }

        [HttpGet]
        public async Task<IActionResult> Download([FromQuery] string fileName)
        {
            var result = await fileService.DownloadAsync(new FileDownloadRequest { FileName = fileName });

            if (!result.IsSuccess)
            {
                return this.ToActionResult(result)?.Result ?? Content("Dosya bulunamadı.");
            }

            var file = result.Value;

            return File(file.Content, file.ContentType, file.Name);
        }

        [HttpDelete]
        public async Task<IActionResult> Delete([FromQuery] string fileName)
        {
            var result = await fileService.DeleteAsync(new FileDeleteRequest { FileName = fileName });

            return this.ToActionResult(result);
        }
    }
}
