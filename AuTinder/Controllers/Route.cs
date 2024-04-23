using AuTinder.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace AuTinder.Controllers
{
    public class Route : Controller
    {
        private readonly ILogger<Route> _logger;

        public Route(ILogger<Route> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }
        public IActionResult AdCreation() 
        {
            return View();
        }
        public IActionResult Ad() 
        {
            return View();
        }
        public IActionResult AdDelete() 
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
