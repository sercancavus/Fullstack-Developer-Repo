using System.Diagnostics;
using App.Mvc.Models;
using App.Mvc.Services;
using Microsoft.AspNetCore.Mvc;

namespace App.Mvc.Controllers
{
    public class HomeController : Controller
    {
        private readonly SingletonService _singleton;
        private readonly ScopedService _scoped;
        private readonly TransientService _transient;
        private readonly DependentService<SingletonService> _dependentSingleton;
        private readonly DependentService<ScopedService> _dependentScoped;
        private readonly DependentService<TransientService> _dependentTransient;

        public HomeController(SingletonService singleton,
            ScopedService scoped,
            TransientService transient,
            DependentService<SingletonService> dependentSingleton,
            DependentService<ScopedService> dependentScoped,
            DependentService<TransientService> dependentTransient)
        {
            _singleton = singleton;
            _scoped = scoped;
            _transient = transient;
            _dependentSingleton = dependentSingleton;
            _dependentScoped = dependentScoped;
            _dependentTransient = dependentTransient;
        }

        public IActionResult Index()
        {
            ViewBag.SingletonCounter = _singleton.GetCounter();
            ViewBag.ScopedCounter = _scoped.GetCounter();
            ViewBag.TransientCounter = _transient.GetCounter();

            ViewBag.DependentSingletonCounter = _dependentSingleton.GetOtherServiceCounter();
            ViewBag.DependentScopedCounter = _dependentScoped.GetOtherServiceCounter();
            ViewBag.DependentTransientCounter = _dependentTransient.GetOtherServiceCounter();


            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
