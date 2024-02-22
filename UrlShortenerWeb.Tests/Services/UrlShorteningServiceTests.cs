using Microsoft.EntityFrameworkCore;
using UrlShortenerWeb.Data;
using UrlShortenerWeb.Models;
using UrlShortenerWeb.Services;

namespace UrlShortenerWeb.Tests.Services
{
    public class UrlShorteningServiceTests
    {
        private readonly DbContextOptions<AppDbContext> _options;

        public UrlShorteningServiceTests()
        {
            _options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase")
                .Options;
        }
        [Fact]
        public void GetAll_ReturnsEmptyList_WhenNoShortUrlsExist()
        {
            using (var context = new AppDbContext(_options))
            {
                // Arrange: Clear existing ShortUrls
                context.ShortUrls.RemoveRange(context.ShortUrls);
                context.SaveChanges();
            }

            using (var context = new AppDbContext(_options))
            {
                // Act
                var service = new UrlShorteningService(context);
                var result = service.GetAll();

                // Assert
                Assert.NotNull(result);
                Assert.Empty(result);
            }
        }

        [Fact]
        public void GetAll_ReturnsAllShortUrls_WhenShortUrlsExist()
        {
            // Arrange
            var shortUrls = new List<ShortUrl>
            {
                new ShortUrl { Id = 1, OriginalUrl = "https://example1.com", Code = "abc123", CreatedBy = "user1", ShortenedUrl = "https://short.url/abc123" },
                new ShortUrl { Id = 2, OriginalUrl = "https://example2.org", Code = "xyz123", CreatedBy = "user2", ShortenedUrl = "https://short.url/xyz123" }
            };
            var expectedCount = 0;

            using (var context = new AppDbContext(_options))
            {
                context.ShortUrls.AddRange(shortUrls);
                context.SaveChanges();
                expectedCount = context.ShortUrls.Count();
            }

            // Act
            using (var context = new AppDbContext(_options))
            {
                var service = new UrlShorteningService(context);
                var result = service.GetAll();

                // Assert
                Assert.NotNull(result);
                Assert.Equal(expectedCount, result.Count());
            }
        }

        [Fact]
        public void GetById_ReturnsShortUrl_WhenIdExists()
        {
            using (var context = new AppDbContext(_options))
            {
                // Arrange
                context.ShortUrls.AddRange(new List<ShortUrl>
                {
                    new ShortUrl { Id = 3, OriginalUrl = "https://example3.com", Code = "abc456", CreatedBy = "user3", ShortenedUrl = "https://short.url/abc456" },
                    new ShortUrl { Id = 4, OriginalUrl = "https://example4.org", Code = "xyz456", CreatedBy = "user4", ShortenedUrl = "https://short.url/xyz456" }
                });
                context.SaveChanges();
            }
            using (var context = new AppDbContext(_options))
            {
                // Act
                var service = new UrlShorteningService(context);
                int id = 3;
                var result = service.GetById(id);

                // Assert
                Assert.NotNull(result);
                Assert.Equal(id, result.Id);
            }
        }

        [Fact]
        public void GetById_ReturnsNull_WhenIdDoesNotExist()
        {
            using (var context = new AppDbContext(_options))
            {
                // Arrange
                // No ShortUrls added to the context

                // Act
                var service = new UrlShorteningService(context);
                int nonExistentId = 999;
                var result = service.GetById(nonExistentId);

                // Assert
                Assert.Null(result);
            }
        }

        [Fact]
        public void GetByOriginalUrl_ReturnsShortUrl_WithCorrectOriginalUrl()
        {
            using (var context = new AppDbContext(_options))
            {
                // Arrange
                context.ShortUrls.AddRange(new List<ShortUrl>
                {
                    new ShortUrl { Id = 5, OriginalUrl = "https://example5.com", Code = "abc789", CreatedBy = "user5", ShortenedUrl = "https://short.url/abc789" },
                    new ShortUrl { Id = 6, OriginalUrl = "https://example6.org", Code = "xyz789", CreatedBy = "user6", ShortenedUrl = "https://short.url/xyz789" }
                });
                context.SaveChanges();
            }

            using (var context = new AppDbContext(_options))
            {
                // Act
                var service = new UrlShorteningService(context);
                var originalUrl = "https://example5.com";
                var result = service.GetByOriginalUrl(originalUrl);

                // Assert
                Assert.NotNull(result);
                Assert.Equal(originalUrl, result.OriginalUrl);
            }
        }

        [Fact]
        public void GetByOriginalUrl_ReturnsShortUrl_WhenShortUrlExists()
        {
            // Arrange
            var originalUrl = "https://example7.com";
            var shortUrl = new ShortUrl { Id = 7, OriginalUrl = originalUrl, Code = "abc777", CreatedBy = "user7", ShortenedUrl = "https://short.url/abc777" };

            using (var context = new AppDbContext(_options))
            {
                context.ShortUrls.Add(shortUrl);
                context.SaveChanges();
            }

            // Act
            using (var context = new AppDbContext(_options))
            {
                var service = new UrlShorteningService(context);
                var result = service.GetByOriginalUrl(originalUrl);

                // Assert
                Assert.NotNull(result);
                Assert.Equal(originalUrl, result.OriginalUrl);
            }
        }

        [Fact]
        public void GetByOriginalUrl_ReturnsNull_WhenShortUrlDoesNotExist()
        {
            // Arrange
            var originalUrl = "https://example.com";

            using (var context = new AppDbContext(_options))
            {
                // Ensure no ShortUrl with the specified original URL exists
                context.ShortUrls.RemoveRange(context.ShortUrls);
                context.SaveChanges();
            }

            // Act
            using (var context = new AppDbContext(_options))
            {
                var service = new UrlShorteningService(context);
                var result = service.GetByOriginalUrl(originalUrl);

                // Assert
                Assert.Null(result);
            }
        }

        [Fact]
        public void Save_AddsShortUrlToDatabase_WhenShortUrlIsValid()
        {
            // Arrange
            var shortUrl = new ShortUrl
            {
                OriginalUrl = "https://example.com",
                ShortenedUrl = "https://short.url/abc123",
                Code = "abc123",
                CreatedBy = "user1"
            };

            using (var context = new AppDbContext(_options))
            {
                // Act
                var service = new UrlShorteningService(context);
                service.Save(shortUrl);

                // Assert
                var savedShortUrl = context.ShortUrls.Find(shortUrl.Id);
                Assert.NotNull(savedShortUrl);
                Assert.Equal(shortUrl.OriginalUrl, savedShortUrl.OriginalUrl);
                Assert.Equal(shortUrl.ShortenedUrl, savedShortUrl.ShortenedUrl);
            }
        }

        [Fact]
        public void DeleteById_RemovesShortUrlFromDatabase_WhenIdExists()
        {
            // Arrange
            var shortUrls = new List<ShortUrl>
            {
                new ShortUrl { Id = 8, OriginalUrl = "https://example8.com", Code = "abc888", CreatedBy = "user8", ShortenedUrl = "https://short.url/abc888" },
                new ShortUrl { Id = 9, OriginalUrl = "https://example9.org", Code = "xyz999", CreatedBy = "user9", ShortenedUrl = "https://short.url/xyz999" }
            };

            using (var context = new AppDbContext(_options))
            {
                context.ShortUrls.AddRange(shortUrls);
                context.SaveChanges();
            }

            using (var context = new AppDbContext(_options))
            {
                // Act
                var service = new UrlShorteningService(context);
                var idToDelete = 8;
                service.DeleteById(idToDelete);

                // Assert
                var deletedShortUrl = context.ShortUrls.Find(idToDelete);
                Assert.Null(deletedShortUrl);
            }
        }

        [Fact]
        public void DeleteAllUrl_RemovesAllShortUrlsFromDatabase()
        {
            // Arrange
            var shortUrls = new List<ShortUrl>
            {
                new ShortUrl { Id = 12, OriginalUrl = "https://example12.com", Code = "abc122", CreatedBy = "user12", ShortenedUrl = "https://short.url/abc122" },
                new ShortUrl { Id = 13, OriginalUrl = "https://example13.org", Code = "xyz133", CreatedBy = "user13", ShortenedUrl = "https://short.url/xyz133" }
            };

            using (var context = new AppDbContext(_options))
            {
                context.ShortUrls.AddRange(shortUrls);
                context.SaveChanges();
            }

            using (var context = new AppDbContext(_options))
            {
                // Act
                var service = new UrlShorteningService(context);
                service.DeleteAllUrl();

                // Assert
                var existingShortUrls = context.ShortUrls.ToList();
                Assert.Empty(existingShortUrls);
            }
        }
    }
}
