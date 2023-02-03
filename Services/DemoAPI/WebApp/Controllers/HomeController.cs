using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System.Diagnostics;
using WebApp.Models;
using WebApp.Models.Settings;

namespace WebApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        private readonly StyleSettings _settings;

        public HomeController(IOptionsSnapshot<StyleSettings> options, ILogger<HomeController> logger)
        {
            _logger = logger;
            _settings = options.Value;
        }

        public IActionResult Index()
        {
            return View(new IndexViewModel(_settings));
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