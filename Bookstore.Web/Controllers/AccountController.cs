using Microsoft.AspNetCore.Mvc;

namespace Bookstore.Web.Controllers;

public class AccountController : Controller
{
    // GET
    public IActionResult Index()
    {
        return View();
    }
}