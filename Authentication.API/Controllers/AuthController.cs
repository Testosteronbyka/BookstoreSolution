using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Authentication.API.Models;

namespace Authentication.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;

        public AuthController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            var user = await _userManager.FindByEmailAsync(request.Email);
            if (user == null)
                return Unauthorized(new { success = false, message = "Nie znaleziono użytkownika" });

            var result = await _signInManager.PasswordSignInAsync(
                user.UserName, // Użyj UserName znalezionego usera!
                request.Password,
                isPersistent: false,
                lockoutOnFailure: false);

            if (result.Succeeded)
                return Ok(new { success = true, message = "Zalogowano pomyślnie" });
            else
                return Unauthorized(new { success = false, message = "Nieprawidłowe dane logowania" });
        }
    }

    public class LoginRequest
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }
}
