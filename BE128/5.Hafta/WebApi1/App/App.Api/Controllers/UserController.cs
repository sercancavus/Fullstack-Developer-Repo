using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace App.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {

        [HttpGet(("name"))]

        public string UserName()
        {
            return "Murat Yılmaz";
        }

        [HttpGet("namebold")]
        public string UserNameBold()
        {
            return "<b>Murat Yılmaz</b>";
        }


        [HttpGet("namebold2")]
        public IActionResult UserNameBold2()
        {
            return Content("<b>Murat Yılmaz</b>", "text/html; charset=utf-8");
        }

        [HttpGet("namebold3")]
        public IActionResult UserNameBold3()
        {
            // Anonim oject(bir classtan türememiş objeler)
            var user = new
            {
                Name = "Mahmut",
                Surname = "Kaya"
            };
            //Ok() => Geriye 200 status code ile bir response döndüğümüzü belirtir
            return Ok(user);
        }

        [HttpGet("namebold4")]
        public IActionResult UserNameBold4()
        {
            // Redirection (Yönlendirme)
            // RedirectToAction() => Geriye 302 status code ile bir response döndüğümüzü belirtir
            // ve belirtilen action'a yönlendirir.
            //return RedirectToAction("UserNameBold");
            //return RedirectToAction("UserNameBold", "Product");
            return RedirectToAction(nameof(UserNameBold));
        }

        [HttpGet("google")]
        public IActionResult Google()
        {
            // Redirect to an external URL
            return Redirect("https://www.google.com");
        }




    }
}
