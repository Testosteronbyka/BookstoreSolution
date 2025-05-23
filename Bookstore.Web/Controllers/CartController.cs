using Microsoft.AspNetCore.Mvc;
using Bookstore.Web.Models;
using Bookstore.Web.Extensions;

namespace Bookstore.Web.Controllers
{
    public class CartController : Controller
    {
        private readonly IHttpClientFactory _clientFactory;

        public CartController(IHttpClientFactory clientFactory)
        {
            _clientFactory = clientFactory;
        }

        [HttpPost]
        public async Task<IActionResult> AddToCart(int bookId)
        {
            try
            {
                var client = _clientFactory.CreateClient("BookAPI");
                var response = await client.GetAsync($"api/books/{bookId}");

                if (response.IsSuccessStatusCode)
                {
                    var book = await response.Content.ReadFromJsonAsync<Book>();
                    if (book != null)
                    {
                        var cart = HttpContext.Session.Get<List<CartItem>>("Cart") ?? new List<CartItem>();
                        
                        var existingItem = cart.FirstOrDefault(c => c.Book.Id == bookId);
                        if (existingItem != null)
                        {
                            existingItem.Quantity++;
                        }
                        else
                        {
                            cart.Add(new CartItem { Book = book, Quantity = 1 });
                        }
                        
                        HttpContext.Session.Set("Cart", cart);
                    }
                }
            }
            catch (Exception ex)
            {
                TempData["Error"] = $"Błąd dodawania do koszyka: {ex.Message}";
            }
            
            return RedirectToAction("Index", "Home");
        }

        public IActionResult Index()
        {
            var cart = HttpContext.Session.Get<List<CartItem>>("Cart") ?? new List<CartItem>();
            return View(cart);
        }

        [HttpPost]
        public IActionResult RemoveFromCart(int bookId)
        {
            var cart = HttpContext.Session.Get<List<CartItem>>("Cart") ?? new List<CartItem>();
            var item = cart.FirstOrDefault(c => c.Book.Id == bookId);
            
            if (item != null)
            {
                cart.Remove(item);
                HttpContext.Session.Set("Cart", cart);
            }
            
            return RedirectToAction("Index");
        }
    }
}
