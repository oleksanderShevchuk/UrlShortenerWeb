using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using UrlShortenerWeb.Data;
using UrlShortenerWeb.DTO;
using UrlShortenerWeb.Services;

namespace UrlShortenerWeb.Controllers
{
    //[Authorize(Roles = Roles.Admin)]
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
        [AllowAnonymous]
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
        [HttpPut("description/edit")]
        public async Task<IActionResult> EditDescription(DescriptionEditDto descriptionDto)
        {
            await _descriptionService.EditDescriptionAsync(descriptionDto);
            return Ok(); 
        }
        [HttpPost("urls/delete-all")]
        public IActionResult DeleteAllUrls()
        {
            // Check if the user has the required role
            var role = User.IsInRole(Roles.Admin);
            if (!User.IsInRole(Roles.Admin))
            {
                return StatusCode(403); // Forbidden status code
            }
            _urlService.DeleteAllUrl();
            return Ok();
        }
    }
}
