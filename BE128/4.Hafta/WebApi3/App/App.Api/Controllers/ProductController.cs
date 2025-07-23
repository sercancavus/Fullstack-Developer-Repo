using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace App.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        // 1)
        //[HttpGet]
        //public string Asus()
        //{
        //    return "Asus Notebook";
        //}

        // 2)

        [HttpGet("asus")]
        public string Asus()
        {
            return "Asus Notebook";
        }

        [HttpGet("hp")]
        public string HP()
        {
            return "HP Notebook";
        }

        // 3)

                [HttpGet("{brand}")]
        public string Notebook(string brand)
        {
            return $"{brand} Notebook";
        }
    }
}
