using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;
using UrlShortenerWeb.Data;
using UrlShortenerWeb.Filters;
using UrlShortenerWeb.Interfaces;
using UrlShortenerWeb.Seeds;
using UrlShortenerWeb.Services;

var builder = WebApplication.CreateBuilder(args);
var connectionString = builder.Configuration.GetConnectionString("AppDbContextConnection") ?? throw new InvalidOperationException("Connection string 'AppDbContextConnection' not found.");

builder.Services.AddDbContext<AppDbContext>(options => options.UseSqlServer(connectionString));

builder.Services.AddDefaultIdentity<ApplicationUser>(options => options.SignIn.RequireConfirmedAccount = false)
    .AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<AppDbContext>()
    .AddRoleManager<RoleManager<IdentityRole>>() // be able to make use of RoleManager
    .AddSignInManager<SignInManager<ApplicationUser>>() // make use of Signin manager
    .AddUserManager<UserManager<ApplicationUser>>() // make use of UserManager to create users
    .AddDefaultTokenProviders(); // be able to create tokens for email confirmation

// be able to authenticate users using JWT
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
            ValidateAudience = false
        };
    });

builder.Services.AddAuthorization(o =>
{
    // Only admin can access.
    o.AddPolicy(Roles.Admin, p => p.RequireRole(Roles.Admin));
});

builder.Services.AddSwaggerGen();

builder.Services.AddAntiforgery(options => options.HeaderName = "X-CSRF-Token");
builder.Services.AddHttpContextAccessor();
builder.Services.AddSingleton<JWTService>();
builder.Services.AddControllers();

builder.Services.AddControllersWithViews(option => option.Filters.Add(new ValidationFilter()));

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddRazorPages();

builder.Services.AddScoped<IUrlShorteningService, UrlShorteningService>();
builder.Services.AddScoped<IDescriptionService, DescriptionService>();
builder.Services.AddScoped<JWTService>();
builder.Services.AddScoped<IdentitySeed>();
builder.Services.AddScoped<DescriptionSeed>();

builder.Services.AddAutoMapper(typeof(Program));

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAllOrigins",
        builder =>
        {
            builder.AllowAnyOrigin()
                   .AllowAnyMethod()
                   .AllowAnyHeader();
        });
});

builder.Services.Configure<ApiBehaviorOptions>(options =>
{
    options.InvalidModelStateResponseFactory = actionContext =>
    {
        var errors = actionContext.ModelState
        .Where(x => x.Value.Errors.Count > 0)
        .SelectMany(x => x.Value.Errors)
        .Select(x => x.ErrorMessage).ToArray();

        var toReturn = new
        {
            Errors = errors
        };

        return new BadRequestObjectResult(toReturn);
    };
});

builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "ShortenerUrl", Version = "v1" });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseCors("AllowAllOrigins");

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}
app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.MapRazorPages();

app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "ShortenerUrl");
    c.RoutePrefix = string.Empty; // Set Swagger UI at the root URL
});

using (var scope = app.Services.CreateScope())
{
    // Add admin or user role if It's not exist
    var identitySeed = scope.ServiceProvider.GetRequiredService<IdentitySeed>();
    await identitySeed.seedDefaultIdentities();

    // Seed descriptions asynchronously
    var descriptionSeed = scope.ServiceProvider.GetRequiredService<DescriptionSeed>();
    await descriptionSeed.SeedDescriptionsAsync();
}

// The functionality of switching to the original URL using a short URL
RedirectToOriginalUrl(app);

app.Run();

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