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
            var books = await GetBooksFromAPI();
            return View(books);
        }

        public async Task<IActionResult> Bookstore()
        {
            var books = await GetBooksFromAPI();
            return View(books);
        }

        private async Task<List<Book>> GetBooksFromAPI()
        {
            try
            {
                var client = _clientFactory.CreateClient("BookAPI");
                _logger.LogInformation(" Próba połączenia z BookStore.API...");
                
                // Sprawdź czy API odpowiada
                var testResponse = await client.GetAsync("test");
                _logger.LogInformation($" Test endpoint: {testResponse.StatusCode}");
                
                var response = await client.GetAsync("api/books");
                _logger.LogInformation($" Books endpoint: {response.StatusCode}");
                
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    _logger.LogInformation($" Odpowiedź API: {content}");
                    
                    var books = await response.Content.ReadFromJsonAsync<List<Book>>();
                    _logger.LogInformation($" Pobrano {books?.Count ?? 0} książek");
                    return books ?? new List<Book>();
                }
                else
                {
                    _logger.LogWarning($" API błąd: {response.StatusCode}");
                    var errorContent = await response.Content.ReadAsStringAsync();
                    _logger.LogWarning($" Błąd content: {errorContent}");
                }
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError(ex, " Błąd połączenia HTTP: {Message}", ex.Message);
            }
            catch (TaskCanceledException ex)
            {
                _logger.LogError(ex, "Timeout połączenia: {Message}", ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, " Nieoczekiwany błąd: {Message}", ex.Message);
            }
            
            return new List<Book>();
        }
    }
}
