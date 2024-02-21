using UrlShortenerWeb.Models;

namespace UrlShortenerWeb.Services
{
    public interface IUrlShorteningService
    {
        ShortUrl GetById(int id);
        ShortUrl GetByOriginalUrl(string originalUrl);
        int Save(ShortUrl shortUrl);
        void DeleteById(int id);
        void DeleteAllUrl();
        string GenerateUniqueUrl();
    }
}