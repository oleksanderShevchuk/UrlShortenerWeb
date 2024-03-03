using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UrlShortenerWeb.Data;
using UrlShortenerWeb.DTO;
using UrlShortenerWeb.Interfaces;

namespace UrlShortenerWeb.Controllers
{
    [Authorize(Roles = Roles.Admin)]
    [Route("api/[controller]")]
    [ApiController]
    public class AdminController : Controller
    {
        private readonly IUrlShorteningService _urlService;
        private readonly IDescriptionService _descriptionService;

        public AdminController(IUrlShorteningService urlService, IDescriptionService descriptionService)
        {
            _urlService = urlService;
            _descriptionService = descriptionService;
        }

        [HttpPut("description/edit")]
        public async Task<IActionResult> EditDescription(DescriptionEditDto descriptionDto)
        {
            await _descriptionService.EditDescriptionAsync(descriptionDto);
            return Ok();
        }
        [HttpPost("urls/delete-all")]
        public IActionResult DeleteAllUrls()
        {
            _urlService.DeleteAllUrl();
            return Ok();
        }
    }
}
