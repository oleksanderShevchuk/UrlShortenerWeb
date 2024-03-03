using System.ComponentModel.DataAnnotations;

namespace UrlShortenerWeb.DTO
{
    public class LoginDto
    {
        [Required(ErrorMessage = "Email is required")]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }
    }
}
