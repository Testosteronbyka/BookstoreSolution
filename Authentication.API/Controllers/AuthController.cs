using Microsoft.AspNetCore.Mvc;
using Authentication.API.Data;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using System.Text;

namespace Authentication.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly AuthDbContext _context;
        private readonly ILogger<AuthController> _logger;

        public AuthController(AuthDbContext context, ILogger<AuthController> logger)
        {
            _context = context;
            _logger = logger;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            try
            {
                _logger.LogInformation($"Próba logowania: {request.Email}");

                // Proste sprawdzenie - admin/admin123
                if (request.Email == "admin@example.com" && request.Password == "admin123")
                {
                    _logger.LogInformation(" Logowanie admin udane");
                    return Ok(new { success = true, message = "Zalogowano pomyślnie" });
                }

                // Sprawdź innych użytkowników (jeśli tabela istnieje)
                try
                {
                    var userExists = await _context.Users.AnyAsync(u => u.Email == request.Email);
                    if (userExists)
                    {
                        _logger.LogInformation(" Użytkownik znaleziony w bazie");
                        return Ok(new { success = true, message = "Zalogowano pomyślnie" });
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogWarning($"Tabela Users nie istnieje: {ex.Message}");
                }

                _logger.LogWarning(" Nieprawidłowe dane logowania");
                return Unauthorized(new { success = false, message = "Nieprawidłowe dane logowania" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Błąd podczas logowania");
                return StatusCode(500, new { success = false, message = $"Błąd serwera: {ex.Message}" });
            }
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequest request)
        {
            try
            {
                _logger.LogInformation($"Próba rejestracji: {request.Email}");
                
                // Proste zaakceptowanie rejestracji
                _logger.LogInformation(" Rejestracja udana");
                return Ok(new { success = true, message = "Konto utworzone pomyślnie" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Błąd podczas rejestracji");
                return StatusCode(500, new { success = false, message = $"Błąd serwera: {ex.Message}" });
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
