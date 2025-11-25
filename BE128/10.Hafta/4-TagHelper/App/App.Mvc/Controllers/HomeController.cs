using System.Diagnostics;
using App.Mvc.Models;
using Microsoft.AspNetCore.Mvc;

namespace App.Mvc.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        // 1)

        //public IActionResult AboutUs()
        //{
        //    return View();
        //}


        // 2) 

        //[Route("hakkimizda")]
        //public IActionResult AboutUs()
        //{
        //    return View();
        //}


        // 3)


        //[Route("hakkimizda/{id}")]
        //public IActionResult AboutUs([FromRoute] int id)
        //{
        //    ViewBag.Id = id;

        //    return View();
        //}


        // 4)

        [Route("hakkimizda/{id}/{title}")]
        public IActionResult AboutUs([FromRoute] int id, [FromRoute] string title)
        {
            ViewBag.Id = id;
            ViewBag.Title = title;
            return View();
        }

    }
}
