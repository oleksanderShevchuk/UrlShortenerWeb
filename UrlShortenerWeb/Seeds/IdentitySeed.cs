using System;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Identity;
using UrlShortenerWeb.Data;

namespace UrlShortenerWeb.Seeds
{
    public class IdentitySeed
    {
        private readonly IServiceProvider _serviceProvider;

        public IdentitySeed(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public async Task seedDefaultIdentities()
        {
            using (var scope = _serviceProvider.CreateScope())
            {
                try
                {
                    var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
                    var roles = new[] { Roles.Admin, Roles.User };

                    foreach (var role in roles)
                    {
                        if (!await roleManager.RoleExistsAsync(role))
                        {
                            await roleManager.CreateAsync(new IdentityRole(role));
                        }
                    }
                }
                catch (Exception ex)
                {
                    var logger = scope.ServiceProvider.GetRequiredService<ILogger<IdentitySeed>>();
                    logger.LogError(ex, "An error occurred while seeding roles.");
                }
            }
        }
    }
}
