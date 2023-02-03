using Microsoft.AspNetCore.Mvc;
using Microsoft.FeatureManagement.Mvc;
using WebApp.Models.Settings;

namespace WebApp.Controllers
{
    [FeatureGate(FeatureFlags.IsBeta)]
    public class BetaController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
