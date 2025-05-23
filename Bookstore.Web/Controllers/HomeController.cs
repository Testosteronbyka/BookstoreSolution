using Microsoft.AspNetCore.Mvc;
using Bookstore.Web.Models;

namespace Bookstore.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly IHttpClientFactory _clientFactory;

        public HomeController(IHttpClientFactory clientFactory)
        {
            _clientFactory = clientFactory;
        }

        public async Task<IActionResult> Index()
        {
            try
            {
                var client = _clientFactory.CreateClient("BookAPI");
                var response = await client.GetAsync("api/books");
                
                if (response.IsSuccessStatusCode)
                {
                    var books = await response.Content.ReadFromJsonAsync<List<Book>>();
                    return View(books ?? new List<Book>());
                }
            }
            catch (Exception ex)
            {
                ViewBag.Error = $"Błąd połączenia z API: {ex.Message}";
            }
            
            return View(new List<Book>());
        }

        public async Task<IActionResult> Bookstore()
        {
            try
            {
                var client = _clientFactory.CreateClient("BookAPI");
                var response = await client.GetAsync("api/books");
                
                if (response.IsSuccessStatusCode)
                {
                    var books = await response.Content.ReadFromJsonAsync<List<Book>>();
                    return View(books ?? new List<Book>());
                }
            }
            catch (Exception ex)
            {
                ViewBag.Error = $"Błąd połączenia z API: {ex.Message}";
            }
            
            return View(new List<Book>());
        }
    }
}