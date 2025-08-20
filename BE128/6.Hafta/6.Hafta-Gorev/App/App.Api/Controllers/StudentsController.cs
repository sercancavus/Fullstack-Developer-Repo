using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")]
public class StudentsController : ControllerBase
{
    private readonly StudentRepository _repository;
    public StudentsController(StudentRepository repository)
    {
        _repository = repository;
    }

    [HttpGet]
    public IActionResult GetAll() => Ok(_repository.GetAll());

    [HttpGet("{id}")]
    public IActionResult GetById(int id)
    {
        var student = _repository.GetById(id);
        return student == null ? NotFound() : Ok(student);
    }

    [HttpPost]
    public IActionResult Add([FromBody] Student student)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);
        try
        {
            var created = _repository.Add(student);
            return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { error = ex.Message });
        }
    }

    [HttpPut("{id}")]
    public IActionResult Update(int id, [FromBody] Student student)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);
        try
        {
            var updated = _repository.Update(id, student);
            return updated ? NoContent() : NotFound();
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { error = ex.Message });
        }
    }

    [HttpDelete("{id}")]
    public IActionResult Delete(int id)
    {
        var deleted = _repository.Delete(id);
        return deleted ? NoContent() : NotFound();
    }
}
