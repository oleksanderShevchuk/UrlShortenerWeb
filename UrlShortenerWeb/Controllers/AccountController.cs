using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using UrlShortenerWeb.Data;
using UrlShortenerWeb.DTO;
using UrlShortenerWeb.Services;

namespace UrlShortenerWeb.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly JWTService _jwtService;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _userManager;

        public AccountController(JWTService jwtService,
            SignInManager<ApplicationUser> signInManager,
            UserManager<ApplicationUser> userManager)
        {
            _jwtService = jwtService;
            _signInManager = signInManager;
            _userManager = userManager;
        }

        [Authorize]
        [HttpGet("refresh-user-token")]
        public async Task<ActionResult<AppUserDto>> RefreshUserToken()
        {
            var user = await _userManager.FindByNameAsync(User.FindFirst(ClaimTypes.Email)?.Value);
            return CreateApplicationUserDto(user);
        }

        [HttpPost("login")]
        public async Task<ActionResult<AppUserDto>> Login(LoginDto model)
        {
            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user == null) return Unauthorized("Invalid email or password");

            var result = await _signInManager.CheckPasswordSignInAsync(user, model.Password, false);
            if (!result.Succeeded) return Unauthorized("Invalid email or password");

            return CreateApplicationUserDto(user);
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterDto model)
        {
            if (await CheckEmailExistsAsync(model.Email))
            {
                return BadRequest($"An existing account is using {model.Email}, email addres. Please try with another email address");
            }

            var EmailConfirmed = model.Password == model.ConfirmPassword;
            var userToAdd = new ApplicationUser
            {
                UserName = model.Email,
                Email = model.Email.ToLower(),
                EmailConfirmed = EmailConfirmed,
            };

            // creates a user inside our AspNetUsers table inside our database
            var result = await _userManager.CreateAsync(userToAdd, model.Password);
            if (result.Succeeded)
            {
                await _userManager.AddToRoleAsync(userToAdd, Roles.User);
            }

            return Ok(new JsonResult(new { title = "Account Created", message = "Your account has been created, you can login" }));
        }

        #region Private Helper Methods
        private AppUserDto CreateApplicationUserDto(ApplicationUser user)
        {
            return new AppUserDto
            {
                Id = user.Id,
                Email = user.Email,
                JWT = _jwtService.CreateJWT(user),
                Role = _userManager.GetRolesAsync(user).Result.FirstOrDefault(),
            };
        }

        private async Task<bool> CheckEmailExistsAsync(string email)
        {
            return await _userManager.Users.AnyAsync(x => x.Email == email.ToLower());
        }
        #endregion
    }
}