using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace App.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StudentController : ControllerBase
    {
        static List<Student> students = new List<Student>
        {
            new Student { Id = 1, FirstName = "John", LastName = "Doe" },
            new Student { Id = 2, FirstName = "Jane", LastName = "Smith" }
        };

        // GET: api/student
        [HttpGet]
        public ActionResult<List<Student>> GetStudents()
        {
            return Ok(students);
        }

        // GET: api/student/1
        [HttpGet("{id}")]
        public ActionResult<Student> GetStudent(int id)
        {
            var student = students.FirstOrDefault(s => s.Id == id);
            if (student == null)
            {
                return NotFound();
            }
            return Ok(student);
        }

        // POST: api/student
        [HttpPost]
        public ActionResult<Student> CreateStudent([FromBody] Student student)
        {
            if (student == null || string.IsNullOrWhiteSpace(student.FirstName) || string.IsNullOrWhiteSpace(student.LastName))
            {
                return BadRequest("Invalid student data.");
            }
            student.Id = students.Max(s => s.Id) + 1; // Simple ID generation
            students.Add(student);
            return CreatedAtAction(nameof(GetStudent), new { id = student.Id }, student);
        }
        // PUT: api/student/1
        [HttpPut("{id}")]
        public ActionResult UpdateStudent(int id, [FromBody] Student student)
        {
            if (student == null || string.IsNullOrWhiteSpace(student.FirstName) || string.IsNullOrWhiteSpace(student.LastName))
            {
                return BadRequest("Invalid student data.");
            }
            var existingStudent = students.FirstOrDefault(s => s.Id == id);
            if (existingStudent == null)
            {
                return NotFound();
            }
            existingStudent.FirstName = student.FirstName;
            existingStudent.LastName = student.LastName;
            return NoContent();
        }

        // DELETE: api/student/1
        [HttpDelete("{id}")]
        public ActionResult DeleteStudent(int id)
        {
            var student = students.FirstOrDefault(s => s.Id == id);
            if (student == null)
            {
                return NotFound();
            }
            students.Remove(student);
            return NoContent();
        }

    }
}