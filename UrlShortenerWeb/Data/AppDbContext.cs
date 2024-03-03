using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using UrlShortenerWeb.Models;

namespace UrlShortenerWeb.Data
{
    public class AppDbContext : IdentityDbContext<ApplicationUser>
    {
        public AppDbContext(DbContextOptions options) : base(options) { }
        public DbSet<ApplicationUser> ApplicationUser { get; set; }
        public DbSet<ShortUrl> ShortUrls { get; set; }
        public DbSet<Description> Descriptions { get; set; }
    }
}