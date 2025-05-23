using Microsoft.AspNetCore.Mvc;

namespace Bookstore.Web.Controllers;

public class CartController : Controller
{
    // GET
    public IActionResult Index()
    {
        return View();
    }
}