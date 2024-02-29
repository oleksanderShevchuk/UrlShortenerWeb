using UrlShortenerWeb.Data;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using UrlShortenerWeb.Models;
using UrlShortenerWeb.DTO;

namespace UrlShortenerWeb.Services
{
    public class DescriptionService : IDescriptionService
    {
        private readonly AppDbContext _context;

        public DescriptionService(AppDbContext context)
        {
            _context = context;
        }
        [Authorize(Roles = Roles.Admin)]
        public async Task EditDescriptionAsync(DescriptionEditDto descriptionDto)
        {
            // Retrieve the existing description entity from the database
            var existingDescription = await _context.Descriptions.FindAsync(descriptionDto.Id);

            if (existingDescription == null)
            {
                throw new ArgumentException("Description with the specified ID not found.");
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
