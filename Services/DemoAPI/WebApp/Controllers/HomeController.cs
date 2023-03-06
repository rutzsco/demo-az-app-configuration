using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.FeatureManagement;
using Microsoft.FeatureManagement.FeatureFilters;
using System.Diagnostics;
using System.Text.RegularExpressions;
using WebApp.Models;
using WebApp.Models.Settings;

namespace WebApp.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        private readonly StyleSettings _settings;

        private readonly IFeatureManagerSnapshot _featureManagerSnapshot;

        public HomeController(IFeatureManagerSnapshot featureManagerSnapshot, IOptionsSnapshot<StyleSettings> options, ILogger<HomeController> logger)
        {
            _logger = logger;
            _settings = options.Value;
            _featureManagerSnapshot = featureManagerSnapshot;
        }

        public async Task<IActionResult> Index()
        {
            var isBetaEnabled = await _featureManagerSnapshot.IsEnabledAsync("Beta");
            var isFeature1Enabled = await _featureManagerSnapshot.IsEnabledAsync("Feature1");
            return View(new IndexViewModel(_settings, isFeature1Enabled, isBetaEnabled));
        }

        public async Task<IActionResult> Demo1Async()
        {
            var a = _featureManagerSnapshot.GetFeatureNamesAsync();
            var isEnabled = await _featureManagerSnapshot.IsEnabledAsync("Beta");
            var isFeature1Enabled = await _featureManagerSnapshot.IsEnabledAsync("Feature1");

            TargetingContext targetingContext = new TargetingContext
            {
                UserId = "scott",
                Groups = new[] { "RINGA" }
            };
            var isEnabled2 = await _featureManagerSnapshot.IsEnabledAsync("Beta", targetingContext);
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [AllowAnonymous]
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}