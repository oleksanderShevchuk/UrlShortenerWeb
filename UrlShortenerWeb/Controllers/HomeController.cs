using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using UrlShortenerWeb.Data;
using UrlShortenerWeb.Models;
using static UrlShortenerWeb.Models.Description;

namespace UrlShortenerWeb.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly AppDbContext _context;

        public HomeController(ILogger<HomeController> logger, AppDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult About()
        {
            return View(_context.Descriptions.FirstOrDefault(d =>
                d.Type == Description.DescriptionType.ShorterAlgorithmDescription));
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
