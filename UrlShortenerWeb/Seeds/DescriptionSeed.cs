using UrlShortenerWeb.Data;
using UrlShortenerWeb.Models;

namespace UrlShortenerWeb.Seeds
{
    public class DescriptionSeed
    {
        private readonly IServiceProvider _serviceProvider;
        private const string _contentForDescriptionIfEmpty = "Our URL Shortener employs a unique algorithm to generate " +
            "short equivalents of long URLs. When you submit a long URL to be shortened, our algorithm creates " +
            "a compact and memorable short URL that redirects to the original long URL. This allows you to share " +
            "links more efficiently, especially on platforms with character limits.";

        public DescriptionSeed(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public async Task SeedDescriptionsAsync()
        {
            using var scope = _serviceProvider.CreateScope();
            var services = scope.ServiceProvider;
            try
            {
                var dbContext = services.GetRequiredService<AppDbContext>();
                if (!dbContext.Descriptions.Any(d => d.Type == Description.DescriptionType.ShorterAlgorithmDescription))
                {
                    var description = new Description
                    {
                        Type = Description.DescriptionType.ShorterAlgorithmDescription,
                        Content = _contentForDescriptionIfEmpty,
                        LastUpdatedTime = DateTime.UtcNow,
                    };
                    dbContext.Descriptions.Add(description);
                    await dbContext.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                var logger = services.GetRequiredService<ILogger<DescriptionSeed>>();
                logger.LogError(ex, "An error occurred while seeding descriptions.");
            }
        }
    }
}
