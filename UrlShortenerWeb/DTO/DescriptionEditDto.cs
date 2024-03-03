using System.ComponentModel.DataAnnotations;

namespace UrlShortenerWeb.DTO
{
    public class DescriptionEditDto
    {
        [Required(ErrorMessage = "Description ID is required")]
        [Range(1, int.MaxValue, ErrorMessage = "Description ID must be a positive integer.")]
        public int Id { get; set; }

        public string Content { get; set; } = string.Empty;

        public DateTime LastUpdatedTime { get; set; }
    }
}
