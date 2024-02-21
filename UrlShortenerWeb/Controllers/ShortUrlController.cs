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
        private readonly UrlShorteningService _urlService;
        private readonly UserManager<ApplicationUser> _userManager;
        public ShortUrlController(UrlShorteningService urlService, UserManager<ApplicationUser> userManager)
        {
            _urlService = urlService;
            _userManager = userManager;
        }
        public IActionResult Index()
        {
            return View(_urlService.GetAll());
        }
        [Authorize]
        public IActionResult UrlInfo(int id)
        {
            return View(_urlService.GetById(id));
        }
        [Authorize]
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(string originalUrl)
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
            return RedirectToAction(actionName: nameof(Index), routeValues: new { id = shortUrl.Id });
        }
        [Authorize]
        public IActionResult Delete(int id)
        {
            var item = _urlService.GetById(id);
            if (item == null)
            {
                return NotFound();
            }
            return View(item);
        }
        [HttpPost, ActionName("Delete")]
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
