using AutoMapper;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using UrlShortenerWeb.Areas.Identity.Data;
using UrlShortenerWeb.Data;
using UrlShortenerWeb.Filters;
using UrlShortenerWeb.Models;
using UrlShortenerWeb.Services;

var builder = WebApplication.CreateBuilder(args);
var connectionString = builder.Configuration.GetConnectionString("AppDbContextConnection") ?? throw new InvalidOperationException("Connection string 'AppDbContextConnection' not found.");

builder.Services.AddDbContext<AppDbContext>(options => options.UseSqlServer(connectionString));

builder.Services.AddDefaultIdentity<ApplicationUser>(options => options.SignIn.RequireConfirmedAccount = false)
    .AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<AppDbContext>();

builder.Services.AddAuthorization(o =>
{
    // Only admin can access.
    o.AddPolicy(Roles.Admin, p => p.RequireRole(Roles.Admin));
});

// Add services to the container.
builder.Services.AddControllersWithViews(option => option.Filters.Add(new ValidationFilter()));
builder.Services.AddRazorPages();

builder.Services.AddScoped<UrlShorteningService>();
builder.Services.AddScoped<DescriptionService>();

builder.Services.AddAutoMapper(typeof(Program));

builder.Services.Configure<IdentityOptions>(options =>
{
    options.Password.RequireUppercase = false;
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}
app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.MapRazorPages();

// Add admin or user role if It's not exist
await SeedRolesAsync(app);

// Seed descriptions asynchronously
await SeedDescriptionsAsync(app);

// The functionality of switching to the original URL using a short URL
RedirectToOriginalUrl(app);

app.Run();

static async Task SeedRolesAsync(WebApplication app)
{
    using (var scope = app.Services.CreateScope())
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
            var logger = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();
            logger.LogError(ex, "An error occurred while seeding roles.");
        }
    }
}
static async Task SeedDescriptionsAsync(WebApplication app)
{
    const string contentForDescriptionIfEmpty = "Our URL Shortener employs a unique algorithm to generate " +
        "short equivalents of long URLs. When you submit a long URL to be shortened, our algorithm creates " +
        "a compact and memorable short URL that redirects to the original long URL. This allows you to share " +
        "links more efficiently, especially on platforms with character limits.";
    using var scope = app.Services.CreateScope();
    var services = scope.ServiceProvider;
    try
    {
        var dbContext = services.GetRequiredService<AppDbContext>();
        // Check if descriptions exist, if not, seed them
        if (!dbContext.Descriptions.Any())
        {
            var description = new Description
            {
                Type = Description.DescriptionType.ShorterAlgorithmDescription,
                Content = contentForDescriptionIfEmpty,
                LastUpdatedTime = DateTime.UtcNow,
            };
            dbContext.Descriptions.Add(description);
            await dbContext.SaveChangesAsync();
        }
    }
    catch (Exception ex)
    {
        var logger = services.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "An error occurred while seeding descriptions.");
    }
}

static void RedirectToOriginalUrl(WebApplication app)
{
    app.MapGet("UrlShortener/{code}", async (string code, AppDbContext dbContext) =>
    {
        var shortenedUrl = await dbContext.ShortUrls
            .FirstOrDefaultAsync(s => s.Code == code);

        if (shortenedUrl == null)
        {
            return Results.NotFound();
        }
        return Results.Redirect(shortenedUrl.OriginalUrl);
    });
}