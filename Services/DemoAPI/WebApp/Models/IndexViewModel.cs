using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Options;
using WebApp.Models.Settings;

namespace WebApp.Models
{
    public class IndexViewModel : PageModel
    {
        public StyleSettings Settings { get; }

        public IndexViewModel(StyleSettings settings)
        {
            Settings = settings;
        }
    }
}
