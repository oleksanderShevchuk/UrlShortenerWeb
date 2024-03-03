using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using UrlShortenerWeb.Data;
using UrlShortenerWeb.Interfaces;
using UrlShortenerWeb.Models;

namespace UrlShortenerWeb.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly AppDbContext _context;
        private readonly IDescriptionService _descriptionService;

        public HomeController(ILogger<HomeController> logger, AppDbContext context, IDescriptionService descriptionService)
        {
            _logger = logger;
            _context = context;
            _descriptionService = descriptionService;
        }

        [HttpGet("Index")]
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet("About")]
        public IActionResult About()
        {
            return View(_context.Descriptions.FirstOrDefault(d =>
                d.Type == Description.DescriptionType.ShorterAlgorithmDescription));
        }

        [HttpGet("description/{id}")]
        public IActionResult GetDescription(int id)
        {
            var description = _descriptionService.FindDescriptionById(id);
            if (description == null)
            {
                return NotFound();
            }
            return Ok(description);
        }

        [HttpGet("Error")]
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
