using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Options;
using WebApp.Models.Settings;

namespace WebApp.Models
{
    public class IndexViewModel : PageModel
    {
        public StyleSettings Settings { get; }
        public bool IsEnabledFeature1 { get; }

        public bool IsEnabledBeta { get; }

        public IndexViewModel(StyleSettings settings, bool isEnabledFeature1, bool isEnabledBeta)
        {
            Settings = settings;
            IsEnabledFeature1 = isEnabledFeature1;
            IsEnabledBeta = isEnabledBeta;
        }
    }
}
