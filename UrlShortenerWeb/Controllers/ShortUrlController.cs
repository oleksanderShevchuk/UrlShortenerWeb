using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using UrlShortenerWeb.Models;
using UrlShortenerWeb.Services;
using Microsoft.AspNetCore.Authorization;
using UrlShortenerWeb.Areas.Identity.Data;
using UrlShortenerWeb.Services.Attributes;

namespace UrlShortenerWeb.Controllers
{
    public class ShortUrlController : Controller
    {
        private readonly IUrlShorteningService _urlService;
        private readonly UserManager<ApplicationUser> _userManager;
        public ShortUrlController(IUrlShorteningService urlService, UserManager<ApplicationUser> userManager)
        {
            _urlService = urlService;
            _userManager = userManager;
        }
        [HttpGet]
        public IActionResult Get()
        {
            List<ShortUrl> links = _urlService.GetAll().ToList();
            return Ok(links);
        }
        [HttpGet]
        public IActionResult GetById(int id)
        {
            ShortUrl shortUrl = _urlService.GetById(id);

            if (shortUrl == null)
            {
                return NotFound();
            }

            return Ok(shortUrl);
        }
        [HttpGet]
        [Authorize]
        public IActionResult UrlInfo(int id)
        {
            return View(_urlService.GetById(id));
        }
        public IEnumerable<ShortUrl> getShortUrls()
        {
            return _urlService.GetAll();
        }
        //[Authorize]
        [HttpPost]
        public async Task<IActionResult> Create([FromBody]string originalUrl)
        {
            if (_urlService.GetByOriginalUrl(originalUrl) is not null)
            {
                ModelState.AddModelError(string.Empty, "URL already exists");
                return View();
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
            return Json(new { success = true }); // Return success response
        }
        //get
        [HttpDelete("delete")]
        public IActionResult Delete(int id)
        {
            var item = _urlService.GetById(id);
            if (item == null)
            {
                return NotFound();
            }
            return View(item);
        }

        [Authorize]
        [HttpDelete("delete")]
        public async Task<IActionResult> DeletePost(int id)
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
                return RedirectToAction("Index");
            }
            _urlService.DeleteById(id);
            return RedirectToAction("Index");
        }
    }
}
