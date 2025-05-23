using Microsoft.AspNetCore.Mvc;
using System.Text;
using System.Text.Json;

namespace Bookstore.Web.Controllers
{
    public class AccountController : Controller
    {
        private readonly IHttpClientFactory _clientFactory;

        public AccountController(IHttpClientFactory clientFactory)
        {
            _clientFactory = clientFactory;
        }

        public IActionResult Login()
        {
            // Jeśli użytkownik jest już zalogowany, przekieruj na stronę główną
            if (HttpContext.Session.GetString("IsLoggedIn") == "true")
            {
                return RedirectToAction("Index", "Home");
            }
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(string email, string password)
        {
            try
            {
                var client = _clientFactory.CreateClient("AuthAPI");
                
                var loginData = new
                {
                    email = email,
                    password = password
                };

                var json = JsonSerializer.Serialize(loginData);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var response = await client.PostAsync("login", content);

                if (response.IsSuccessStatusCode)
                {
                    // Zapisz informacje o zalogowaniu w sesji
                    HttpContext.Session.SetString("IsLoggedIn", "true");
                    HttpContext.Session.SetString("UserEmail", email);
                    
                    TempData["Success"] = "Zalogowano pomyślnie!";
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    TempData["Error"] = "Nieprawidłowe dane logowania";
                }
            }
            catch (Exception ex)
            {
                TempData["Error"] = $"Błąd logowania: {ex.Message}";
            }

            return View();
        }

        public IActionResult Register()
        {
            // Jeśli użytkownik jest już zalogowany, przekieruj na stronę główną
            if (HttpContext.Session.GetString("IsLoggedIn") == "true")
            {
                return RedirectToAction("Index", "Home");
            }
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(string email, string password, string confirmPassword)
        {
            if (password != confirmPassword)
            {
                TempData["Error"] = "Hasła nie są identyczne";
                return View();
            }

            try
            {
                var client = _clientFactory.CreateClient("AuthAPI");
                
                var registerData = new
                {
                    email = email,
                    password = password
                };

                var json = JsonSerializer.Serialize(registerData);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var response = await client.PostAsync("register", content);

                if (response.IsSuccessStatusCode)
                {
                    TempData["Success"] = "Konto utworzone pomyślnie! Możesz się teraz zalogować.";
                    return RedirectToAction("Login");
                }
                else
                {
                    var error = await response.Content.ReadAsStringAsync();
                    TempData["Error"] = $"Błąd rejestracji: {error}";
                }
            }
            catch (Exception ex)
            {
                TempData["Error"] = $"Błąd rejestracji: {ex.Message}";
            }

            return View();
        }

        [HttpPost]
        public IActionResult Logout()
        {
            // Wyczyść sesję
            HttpContext.Session.Clear();
            
            TempData["Success"] = "Wylogowano pomyślnie!";
            return RedirectToAction("Index", "Home");
        }
    }
}
