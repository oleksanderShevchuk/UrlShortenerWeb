using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using UrlShortenerWeb.Models;
using UrlShortenerWeb.Services;
using Microsoft.AspNetCore.Authorization;
using UrlShortenerWeb.Data;

namespace UrlShortenerWeb.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ShortUrlController : ControllerBase
    {
        private readonly IUrlShorteningService _urlService;
        private readonly UserManager<ApplicationUser> _userManager;

        public ShortUrlController(IUrlShorteningService urlService, UserManager<ApplicationUser> userManager)
        {
            _urlService = urlService;
            _userManager = userManager;
        }

        [HttpGet("get")]
        public IActionResult Get()
        {
            List<ShortUrl> links = _urlService.GetAll().ToList();
            return Ok(links);
        }

        [HttpGet("get-by-id/{id}")]
        public IActionResult GetById(int id)
        {
            ShortUrl shortUrl = _urlService.GetById(id);
            if (shortUrl == null)
            {
                return NotFound();
            }
            return Ok(shortUrl);
        }

        [HttpGet("url-info/{id}")]
        [Authorize]
        public IActionResult UrlInfo(int id)
        {
            ShortUrl shortUrl = _urlService.GetById(id);
            if (shortUrl == null)
            {
                return NotFound();
            }
            return Ok(shortUrl);
        }

        [Authorize]
        [HttpPost("create")]
        public async Task<IActionResult> Create([FromBody] string originalUrl)
        {
            if (_urlService.GetByOriginalUrl(originalUrl) != null)
            {
                ModelState.AddModelError(string.Empty, "URL already exists");
                return BadRequest(ModelState);
            }
            var user = await _userManager.GetUserAsync(User);
            var code = _urlService.GenerateUniqueUrl();
            var shortUrl = new ShortUrl
            {
                OriginalUrl = originalUrl,
                CreatedBy = user.Id,
                ShortenedUrl = $"{HttpContext.Request.Scheme}://{HttpContext.Request.Host}/UrlShortener/{code}",
                Code = code,
                CreatedDate = DateTime.UtcNow,
            };
            _urlService.Save(shortUrl);
            return Ok(new { success = true }); // Return success response
        }

        [Authorize]
        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var item = _urlService.GetById(id);
            if (item == null)
            {
                return NotFound();
            }
            var user = await _userManager.GetUserAsync(User);
            var role = (await _userManager.GetRolesAsync(user)).FirstOrDefault();

            if (item.CreatedBy != user.Id.ToString() && role != Roles.Admin)
            {
                ModelState.AddModelError(nameof(ShortUrl), "You can not delete someone's URL.");
                return BadRequest(ModelState);
            }
            _urlService.DeleteById(id);
            return Ok(new { success = true }); // Return success response
        }
    }
}
