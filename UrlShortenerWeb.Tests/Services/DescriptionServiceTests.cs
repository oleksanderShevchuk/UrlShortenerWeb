using Microsoft.EntityFrameworkCore;
using UrlShortenerWeb.Data;
using UrlShortenerWeb.Models;
using UrlShortenerWeb.Models.DTO;
using UrlShortenerWeb.Services;

namespace UrlShortenerWeb.Tests.Services
{
    public class DescriptionServiceTests
    {
        private readonly DbContextOptions<AppDbContext> _options;

        public DescriptionServiceTests()
        {
            _options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase")
                .Options;
        }

        [Fact]
        public async Task EditDescriptionAsync_UpdatesDescription_WhenDescriptionExists()
        {
            // Arrange
            var descriptionId = 1;
            var descriptionContent = "Initial description";
            var newDescriptionContent = "Updated description";

            using (var context = new AppDbContext(_options))
            {
                // Add a description to the database
                context.Descriptions.Add(new Description { Id = descriptionId, Content = descriptionContent });
                context.SaveChanges();
            }

            using (var context = new AppDbContext(_options))
            {
                // Act
                var service = new DescriptionService(context);
                var descriptionDto = new DescriptionEditDto { Id = descriptionId, Content = newDescriptionContent };
                await service.EditDescriptionAsync(descriptionDto);

                // Assert
                var updatedDescription = await context.Descriptions.FindAsync(descriptionId);
                Assert.NotNull(updatedDescription);
                Assert.Equal(newDescriptionContent, updatedDescription.Content);
            }
        }

        [Fact]
        public async Task EditDescriptionAsync_ThrowsArgumentException_WhenDescriptionDoesNotExist()
        {
            // Arrange
            var nonExistentDescriptionId = 999;
            var descriptionDto = new DescriptionEditDto { Id = nonExistentDescriptionId, Content = "Updated description" };

            using (var context = new AppDbContext(_options))
            {
                // Act
                var service = new DescriptionService(context);

                // Assert
                await Assert.ThrowsAsync<ArgumentException>(() => service.EditDescriptionAsync(descriptionDto));
            }
        }

        [Fact]
        public void FindDescriptionById_ReturnsDescription_WhenDescriptionExists()
        {
            // Arrange
            var descriptionId = 2;
            var descriptionContent = "Test description";

            using (var context = new AppDbContext(_options))
            {
                // Add a description to the database
                context.Descriptions.Add(new Description { Id = descriptionId, Content = descriptionContent });
                context.SaveChanges();
            }

            using (var context = new AppDbContext(_options))
            {
                // Act
                var service = new DescriptionService(context);
                var foundDescription = service.FindDescriptionById(descriptionId);

                // Assert
                Assert.NotNull(foundDescription);
                Assert.Equal(descriptionId, foundDescription.Id);
                Assert.Equal(descriptionContent, foundDescription.Content);
            }
        }

        [Fact]
        public void FindDescriptionById_ReturnsNull_WhenDescriptionDoesNotExist()
        {
            // Arrange
            var nonExistentDescriptionId = 999;

            using (var context = new AppDbContext(_options))
            {
                // Act
                var service = new DescriptionService(context);
                var foundDescription = service.FindDescriptionById(nonExistentDescriptionId);

                // Assert
                Assert.Null(foundDescription);
            }
        }
    }
}
