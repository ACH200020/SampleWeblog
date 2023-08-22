using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BlogWeb.Pages
{
    [Authorize(Roles = "Downloader")]
    public class DownloadModel : PageModel
    {
        private readonly IWebHostEnvironment _env;

        public DownloadModel(IWebHostEnvironment env)
        {
            _env = env;
        }

        public IActionResult OnGet(string slug)
        {
            var path = Path.Combine(_env.WebRootPath, "PostPdf", $"{slug}.pdf");
            return File(System.IO.File.ReadAllBytes(path),$"{slug}/pdf", System.IO.Path.GetFileName(path));
        }
    }
}
