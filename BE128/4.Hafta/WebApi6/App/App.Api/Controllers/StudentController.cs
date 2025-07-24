using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace App.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StudentController : ControllerBase
    {
        // burada CRUD işlemleri yapılacak, public void gibi işlemler ile devam edilecek

        [HttpGet]
        public IActionResult GetStudents()
        {
            // Örnek veri döndürme
            var students = new List<string> { "Ali", "Ayşe", "Mehmet" };
            return Ok(students);
        }

        [HttpGet("{id}")]
        public IActionResult GetStudent(int id)
        {
            // Örnek veri döndürme
            var student = $"Öğrenci {id}";
            return Ok(student);
        }

        [HttpPost]
        public IActionResult CreateStudent([FromBody] string studentName)
        {
            // Öğrenci oluşturma işlemi
            if (string.IsNullOrEmpty(studentName))
            {
                return BadRequest("Öğrenci adı boş olamaz.");
            }
            // Başarılı yanıt
            return CreatedAtAction(nameof(GetStudent), new { id = 1 }, studentName);
        }

        [HttpPut("{id}")]
        public IActionResult UpdateStudent(int id, [FromBody] string studentName)
        {
            // Öğrenci güncelleme işlemi
            if (string.IsNullOrEmpty(studentName))
            {
                return BadRequest("Öğrenci adı boş olamaz.");
            }
            // Başarılı yanıt
            return NoContent();
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteStudent(int id)
        {
            // Öğrenci silme işlemi
            // Başarılı yanıt
            return NoContent();
        }




    }
}