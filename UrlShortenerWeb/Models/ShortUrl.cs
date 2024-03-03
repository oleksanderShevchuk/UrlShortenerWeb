using System.ComponentModel.DataAnnotations;
using UrlShortenerWeb.Attributes;

namespace UrlShortenerWeb.Models
{
    public class ShortUrl
    {
        [Required]
        [PositiveInteger(ErrorMessage = "URL ID must be a positive integer.")]
        [Range(1, int.MaxValue, ErrorMessage = "Id must be greater than 0.")]
        public int Id { get; set; }
        [Required(ErrorMessage = "Original URL is required")]
        [Url(ErrorMessage = "Please enter a valid URL")]
        public string OriginalUrl { get; set; } 
        public string ShortenedUrl { get; set; }
        public string Code { get; set; }
        public DateTime CreatedDate { get; set; }
        public string CreatedBy { get; set; }
    }
}
