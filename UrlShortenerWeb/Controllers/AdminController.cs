using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using UrlShortenerWeb.Areas.Identity.Data;
using UrlShortenerWeb.Data;
using UrlShortenerWeb.Models;
using UrlShortenerWeb.Models.DTO;
using UrlShortenerWeb.Services;

namespace UrlShortenerWeb.Controllers
{
    public class AdminController : Controller
    {
        private readonly UrlShorteningService _urlService;
        private readonly DescriptionService _descriptionService;

        public AdminController(UrlShorteningService urlService, DescriptionService descriptionService)
        {
            _urlService = urlService;
            _descriptionService = descriptionService;
        }
        [Authorize(Roles = Roles.Admin)]
        public IActionResult EditDescription(int id)
        {
            var description = _descriptionService.FindDescriptionById(id);
            if (description == null)
            {
                return NotFound();
            }
            return View(description);
        }
        [HttpPost]
        public async Task<IActionResult> EditDescription(DescriptionEditDto descriptionDto)
        {
            await _descriptionService.EditDescriptionAsync(descriptionDto);
            return RedirectToAction("About", "Home");
        }
        public IActionResult DeleteAllUrls()
        {
            return View();
        }
        [HttpPost, ActionName("DeleteAllUrls")]
        [Authorize(Roles.Admin)]
        public IActionResult DeleteAllUrlsPost()
        {
            _urlService.DeleteAllUrl();
            return RedirectToAction("Index", "ShortUrl");
        }
    }
}
