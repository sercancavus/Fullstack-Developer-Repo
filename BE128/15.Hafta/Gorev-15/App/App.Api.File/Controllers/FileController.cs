using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.StaticFiles;

namespace App.Api.File.Controllers;

[ApiController]
[Route("api/[controller]")]
public class FileController : ControllerBase
{
    private readonly string _uploadRoot;
    private readonly FileExtensionContentTypeProvider _contentTypeProvider = new();

    public FileController(IWebHostEnvironment env)
    {
        _uploadRoot = Path.Combine(env.ContentRootPath, "Uploads");
        Directory.CreateDirectory(_uploadRoot);
    }

    [HttpPost("upload")]
    public async Task<IActionResult> Upload([FromForm] IFormFile file)
    {
        if (file == null || file.Length == 0)
        {
            return BadRequest("Dosya bulunamadý.");
        }

        var fileName = $"{Guid.NewGuid()}{Path.GetExtension(file.FileName)}";
        var fullPath = Path.Combine(_uploadRoot, fileName);

        await using var stream = new FileStream(fullPath, FileMode.Create);
        await file.CopyToAsync(stream);

        var url = $"/api/file/download?fileName={Uri.EscapeDataString(fileName)}";
        return Created(url, new { fileName, url });
    }

    [HttpGet("download")]
    public IActionResult Download([FromQuery] string fileName)
    {
        if (string.IsNullOrWhiteSpace(fileName))
        {
            return BadRequest();
        }

        var fullPath = Path.Combine(_uploadRoot, fileName);
        if (!System.IO.File.Exists(fullPath))
        {
            return NotFound();
        }

        if (!_contentTypeProvider.TryGetContentType(fullPath, out var contentType))
        {
            contentType = "application/octet-stream";
        }

        var stream = new FileStream(fullPath, FileMode.Open, FileAccess.Read, FileShare.Read);
        return File(stream, contentType, fileName);
    }
}
