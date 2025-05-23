using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Authentication.API.Models;
using Microsoft.EntityFrameworkCore;

namespace Authentication.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ILogger<AuthController> _logger;

        public AuthController(
            UserManager<ApplicationUser> userManager,
            ILogger<AuthController> logger)
        {
            _userManager = userManager;
            _logger = logger;
        }

        [HttpGet("test")]
        public async Task<IActionResult> Test()
        {
            try
            {
                _logger.LogInformation("🧪 Test endpoint");
                
                var users = await _userManager.Users.ToListAsync();
                
                return Ok(new
                {
                    success = true,
                    message = "✅ Authentication API działa",
                    timestamp = DateTime.Now,
                    userCount = users.Count,
                    users = users.Select(u => new {
                        Id = u.Id,
                        Email = u.Email,
                        UserName = u.UserName,
                        EmailConfirmed = u.EmailConfirmed
                    })
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "❌ Błąd test endpoint");
                return StatusCode(500, new
                {
                    success = false,
                    message = "❌ Błąd bazy danych",
                    error = ex.Message
                });
            }
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            try
            {
                _logger.LogInformation($"🔐 Logowanie: {request.Email}");

                if (string.IsNullOrEmpty(request.Email) || string.IsNullOrEmpty(request.Password))
                {
                    return BadRequest(new { success = false, message = "Email i hasło wymagane" });
                }

                var user = await _userManager.FindByEmailAsync(request.Email);
                if (user == null)
                {
                    user = await _userManager.FindByNameAsync(request.Email);
                }

                if (user == null)
                {
                    return Unauthorized(new { success = false, message = "Nieprawidłowe dane" });
                }

                var passwordCheck = await _userManager.CheckPasswordAsync(user, request.Password);
                if (passwordCheck)
                {
                    _logger.LogInformation($"✅ Logowanie OK: {request.Email}");
                    return Ok(new 
                    { 
                        success = true, 
                        message = "Zalogowano pomyślnie",
                        userId = user.Id,
                        email = user.Email
                    });
                }
                else
                {
                    return Unauthorized(new { success = false, message = "Nieprawidłowe dane" });
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "💥 Błąd logowania");
                return StatusCode(500, new { success = false, message = "Błąd serwera" });
            }
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequest request)
        {
            try
            {
                if (string.IsNullOrEmpty(request.Email) || string.IsNullOrEmpty(request.Password))
                {
                    return BadRequest(new { success = false, message = "Email i hasło wymagane" });
                }

                var existingUser = await _userManager.FindByEmailAsync(request.Email);
                if (existingUser != null)
                {
                    return BadRequest(new { success = false, message = "Użytkownik już istnieje" });
                }

                var user = new ApplicationUser
                {
                    UserName = request.Email,
                    Email = request.Email,
                    EmailConfirmed = true
                };

                var result = await _userManager.CreateAsync(user, request.Password);
                if (result.Succeeded)
                {
                    return Ok(new { success = true, message = "Konto utworzone" });
                }
                else
                {
                    var errors = string.Join(", ", result.Errors.Select(e => e.Description));
                    return BadRequest(new { success = false, message = errors });
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "💥 Błąd rejestracji");
                return StatusCode(500, new { success = false, message = "Błąd serwera" });
            }
        }
    }

    public class LoginRequest
    {
        public string Email { get; set; } = "";
        public string Password { get; set; } = "";
    }

    public class RegisterRequest
    {
        public string Email { get; set; } = "";
        public string Password { get; set; } = "";
    }
}
