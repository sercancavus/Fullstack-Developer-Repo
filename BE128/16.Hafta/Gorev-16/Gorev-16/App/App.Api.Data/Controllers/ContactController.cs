using App.Data.Entities;
using App.Data.Infrastructure;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace App.Api.Data.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ContactController(DataRepository<ContactFormEntity> repo) : ControllerBase
{
    public record NewContact(string Name, string Email, string Message);

    [HttpPost]
    [AllowAnonymous]
    public async Task<IActionResult> Create([FromBody] NewContact model)
    {
        if (string.IsNullOrWhiteSpace(model.Name) || string.IsNullOrWhiteSpace(model.Email) || string.IsNullOrWhiteSpace(model.Message))
        {
            return BadRequest();
        }

        var entity = new ContactFormEntity
        {
            Name = model.Name,
            Email = model.Email,
            Message = model.Message,
            SeenAt = null
        };

        var created = await repo.AddAsync(entity);
        return Created($"/api/contact/{created.Id}", new { created.Id });
    }
}
