using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using UrlShortenerWeb.Data;

namespace UrlShortenerWeb.Services
{
    public class JWTService
    {
        private readonly IConfiguration _config;
        private readonly SymmetricSecurityKey _jwtKey;
        private readonly IServiceScopeFactory _scopeFactory;

        public JWTService(IConfiguration config, IServiceScopeFactory scopeFactory)
        {
            _config = config;
            _scopeFactory = scopeFactory;

            // jwtKey is used for both encrypting and decrypting the JWT token
            _jwtKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["JWT:Key"]));
        }

        public string CreateJWT(ApplicationUser user)
        {
            using (var scope = _scopeFactory.CreateScope())
            {
                var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();
                var role = userManager.GetRolesAsync(user).Result.FirstOrDefault();

                var userClaims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Name, user.Email),
                new Claim(ClaimTypes.Role, role)
            };

                var credentials = new SigningCredentials(_jwtKey, SecurityAlgorithms.HmacSha512Signature);
                var tokenDescriptor = new SecurityTokenDescriptor
                {
                    Subject = new ClaimsIdentity(userClaims),
                    Expires = DateTime.UtcNow.AddMinutes(int.Parse(_config["JWT:ExpiresInMinutes"])),
                    SigningCredentials = credentials,
                    Issuer = _config["JWT:Issuer"]
                };

                var tokenHandler = new JwtSecurityTokenHandler();
                var jwt = tokenHandler.CreateToken(tokenDescriptor);
                return tokenHandler.WriteToken(jwt);
            }
        }
    }

}
