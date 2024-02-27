using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Net;
using System.Text;
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

builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Your API", Version = "v1" });
});

// Add services to the container.
//builder.Services.AddControllersWithViews(option => option.Filters.Add(new ValidationFilter()));
builder.Services.AddRazorPages();

builder.Services.AddScoped<IUrlShorteningService, UrlShorteningService>();
builder.Services.AddScoped<IDescriptionService, DescriptionService>();
builder.Services.AddScoped<JWTService>();

builder.Services.AddAutoMapper(typeof(Program));

builder.Services.Configure<IdentityOptions>(options =>
{
    options.Password.RequireUppercase = false;
});

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            // validate the token based on the key we have provided inside appsettings.development.json JWT:Key
            ValidateIssuerSigningKey = true,
            // the issuer singning key based on JWT:Key
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JWT:Key"])),
            // the issuer which in here is the api project url we are using
            ValidIssuer = builder.Configuration["JWT:Issuer"],
            // validate the issuer (who ever is issuing the JWT)
            ValidateIssuer = true,
            // don't validate audience (angular side)
            ValidateAudience = false,
            ValidateLifetime = true,
            ClockSkew = TimeSpan.Zero
        };
    });
builder.Services.AddCors();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}
app.UseHttpsRedirection();

app.UseCors(builder =>
{
    builder.AllowAnyHeader()
           .AllowAnyMethod()
           .AllowCredentials()
           .WithOrigins("http://localhost:4200");
});

app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Your API V1");
});

app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.MapRazorPages();

#pragma warning disable ASP0014
app.UseEndpoints(endpoints =>
{
endpoints.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{Id?}");
    endpoints.MapRazorPages();
});

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