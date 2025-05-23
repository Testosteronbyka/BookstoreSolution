using Microsoft.AspNetCore.Mvc;

namespace BookStore.API.Controllers;

public class BooksController : Controller
{
    // GET
    public IActionResult Index()
    {
        return View();
    }
}