using System;
using System.ComponentModel.DataAnnotations;
using UrlShortenerWeb.Services.Attributes;

namespace UrlShortenerWeb.Models
{
    public class Description
    {
        [Required]
        [Range(1, int.MaxValue)]
        [PositiveInteger(ErrorMessage = "Description ID must be a positive integer.")]
        public int Id { get; set; }
        public DescriptionType Type { get; set; }
        public string Content { get; set; } = String.Empty;

        public DateTime LastUpdatedTime { get; set; }
        public enum DescriptionType
        {
            ShorterAlgorithmDescription = 100
        }
    }
}
