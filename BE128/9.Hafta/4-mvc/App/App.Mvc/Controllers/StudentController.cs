using App.Mvc.Models;
using Microsoft.AspNetCore.Mvc;

namespace App.Mvc.Controllers
{
    public class StudentController : Controller
    {
        public IActionResult Index()
        {
            List<Student> students = new()
            {
                new Student { Id = 1, Name = "Ali" , Surname = "Yılmaz"},
                new Student { Id = 2, Name = "Veli" , Surname = "Yıldız"},
                new Student { Id = 3, Name = "Mahmut" , Surname = "Kaya"},
            };

            ViewBag.Ogrenciler = students;

            return View();
        }
    }
}
