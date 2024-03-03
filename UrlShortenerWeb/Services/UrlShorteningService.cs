using UrlShortenerWeb.Data;
using UrlShortenerWeb.Helpers;
using UrlShortenerWeb.Interfaces;
using UrlShortenerWeb.Models;

namespace UrlShortenerWeb.Services
{
    public class UrlShorteningService : IUrlShorteningService
    {
        private readonly AppDbContext _context;

        public UrlShorteningService(AppDbContext context)
        {
            _context = context;
        }

        public IEnumerable<ShortUrl> GetAll()
        {
            var query = _context.ShortUrls.ToList();
            return query;
        }
        public ShortUrl GetById(int id)
        {
            return _context.ShortUrls.Find(id);
        }
        public ShortUrl GetByOriginalUrl(string originalUrl)
        {
            foreach (var url in _context.ShortUrls) 
            { 
                if (url.OriginalUrl == originalUrl)
                {
                    return url;
                }
            }
            return null;
        }
        public int Save(ShortUrl shortUrl)
        {
            _context.ShortUrls.Add(shortUrl);
            _context.SaveChanges();
            return shortUrl.Id;
        }
        public string GenerateUniqueUrl()
        {
            var code = ShortUrlHelper.GenerateUniqueCode();
            if (!_context.ShortUrls.Any(s => s.ShortenedUrl == code))
            {
                return code;
            }
            return GenerateUniqueUrl();
        }
        public void DeleteById(int id)
        {
            _context.ShortUrls.Remove(GetById(id));
            _context.SaveChanges();
        }
        public void DeleteAllUrl()
        {
            var allUrls = _context.ShortUrls.ToList();
            _context.ShortUrls.RemoveRange(allUrls);
            _context.SaveChanges();
        }
    }
}
