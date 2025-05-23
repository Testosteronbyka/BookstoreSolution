using Microsoft.AspNetCore.Mvc;
using Bookstore.Web.Models;

namespace Bookstore.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly IHttpClientFactory _clientFactory;
        private readonly ILogger<HomeController> _logger;

        public HomeController(IHttpClientFactory clientFactory, ILogger<HomeController> logger)
        {
            _clientFactory = clientFactory;
            _logger = logger;
        }

        public async Task<IActionResult> Index()
        {
            try
            {
                var client = _clientFactory.CreateClient("BookAPI");
                _logger.LogInformation("Próba połączenia z BookStore.API...");
                
                var response = await client.GetAsync("api/books");
                _logger.LogInformation($"Odpowiedź z API: {response.StatusCode}");
                
                if (response.IsSuccessStatusCode)
                {
                    var books = await response.Content.ReadFromJsonAsync<List<Book>>();
                    _logger.LogInformation($"Pobrano {books?.Count ?? 0} książek");
                    return View(books ?? new List<Book>());
                }
                else
                {
                    _logger.LogWarning($"API zwróciło błąd: {response.StatusCode}");
                    ViewBag.Error = $"Błąd API: {response.StatusCode}";
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Błąd podczas pobierania książek");
                ViewBag.Error = $"Błąd połączenia z API: {ex.Message}";
            }
            
            return View(new List<Book>());
        }

        public async Task<IActionResult> Bookstore()
        {
            try
            {
                var client = _clientFactory.CreateClient("BookAPI");
                _logger.LogInformation("Próba połączenia z BookStore.API dla księgarni...");
                
                var response = await client.GetAsync("api/books");
                _logger.LogInformation($"Odpowiedź z API: {response.StatusCode}");
                
                if (response.IsSuccessStatusCode)
                {
                    var books = await response.Content.ReadFromJsonAsync<List<Book>>();
                    _logger.LogInformation($"Pobrano {books?.Count ?? 0} książek dla księgarni");
                    return View(books ?? new List<Book>());
                }
                else
                {
                    _logger.LogWarning($"API zwróciło błąd: {response.StatusCode}");
                    ViewBag.Error = $"Błąd API: {response.StatusCode}";
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Błąd podczas pobierania książek dla księgarni");
                ViewBag.Error = $"Błąd połączenia z API: {ex.Message}";
            }
            
            return View(new List<Book>());
        }
    }
}
