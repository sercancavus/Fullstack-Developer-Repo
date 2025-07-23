using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace WebApi._5.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StudentController : ControllerBase
    {
        List<Student> values = new List<Student>
        {
            new Student { Id = 1, FirstName = "John", LastName = "Doe" },
            new Student { Id = 2, FirstName = "Jane", LastName = "Smith" }
        };

        [HttpGet]
        public ActionResult<IEnumerable<Student>> Get()
        {
            return Ok(values);
        }

        [HttpGet("{id}")]
        public ActionResult<Student> Get(int id)
        {
            var student = values.FirstOrDefault(s => s.Id == id);
            if (student == null)
            {
                return NotFound();
            }
            return Ok(student);
        }

        [HttpPost]
        public ActionResult<Student> Post([FromBody] Student student)
        {
            if (student == null)
            {
                return BadRequest("Student cannot be null");
            }
            student.Id = values.Max(s => s.Id) + 1; // Simple ID generation
            values.Add(student);
            return CreatedAtAction(nameof(Get), new { id = student.Id }, student);
        }

        [HttpPut("{id}")]
        public ActionResult<Student> Put(int id, [FromBody] Student student)
        {
            if (student == null || student.Id != id)
            {
                return BadRequest("Student ID mismatch");
            }
            var existingStudent = values.FirstOrDefault(s => s.Id == id);
            if (existingStudent == null)
            {
                return NotFound();
            }
            existingStudent.FirstName = student.FirstName;
            existingStudent.LastName = student.LastName;
            return Ok(existingStudent);
        }

        [HttpDelete("{id}")]
        public ActionResult Delete(int id)
        {
            var student = values.FirstOrDefault(s => s.Id == id);
            if (student == null)
            {
                return NotFound();
            }
            values.Remove(student);
            return NoContent();
        }

    }
}
