using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UrlShortenerWeb.Data;
using UrlShortenerWeb.DTO;
using UrlShortenerWeb.Services;

namespace UrlShortenerWeb.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
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
        [Authorize(Roles = Roles.Admin)]
        [HttpPost("description/edit")]
        public async Task<IActionResult> EditDescription(DescriptionEditDto descriptionDto)
        {
            await _descriptionService.EditDescriptionAsync(descriptionDto);
            return Ok(); 
        }
        [Authorize(Roles.Admin)]
        [HttpPost("urls/delete-all")]
        public IActionResult DeleteAllUrls()
        {
            _urlService.DeleteAllUrl();
            return Ok();
        }
    }
}
