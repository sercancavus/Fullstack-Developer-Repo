using Microsoft.AspNetCore.Mvc;

namespace App.Mvc.Controllers
{
    public class SessionController : Controller
    {
        public IActionResult Set()
        {
            HttpContext.Session.SetString("Username", "Tufan");
            HttpContext.Session.SetInt32("Age", 50);
            return Ok();
        }
        public IActionResult Get()
        {
            var username = HttpContext.Session.GetString("Username");
            var age = HttpContext.Session.GetInt32("Age");
            return Ok(new { username = username, age =age});
        }
        public IActionResult Delete()
        {
            HttpContext.Session.Remove("Username");
            HttpContext.Session.Remove("Age");

            return Ok();
        }
        public IActionResult Show()
        {
            return View();
        }
    }
}
