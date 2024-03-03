using UrlShortenerWeb.Data;
using UrlShortenerWeb.Models;
using UrlShortenerWeb.DTO;
using UrlShortenerWeb.Interfaces;

namespace UrlShortenerWeb.Services
{
    public class DescriptionService : IDescriptionService
    {
        private readonly AppDbContext _context;

        public DescriptionService(AppDbContext context)
        {
            _context = context;
        }
        public async Task EditDescriptionAsync(DescriptionEditDto descriptionDto)
        {
            // Retrieve the existing description entity from the database
            var existingDescription = await _context.Descriptions.FindAsync(descriptionDto.Id);

            // If the existing description is not found, return without performing any action
            if (existingDescription == null)
            {
                // Return null to indicate that the description was not found
                return;
            }

            // Update the properties of the existing description entity
            existingDescription.Content = descriptionDto.Content;
            existingDescription.LastUpdatedTime = DateTime.UtcNow;

            // Save the changes to the database
            await _context.SaveChangesAsync();
        }

        public Description FindDescriptionById(int id)
        {
            return _context.Descriptions.Find(id);
        }
    }
}
