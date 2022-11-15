using Microsoft.AspNetCore.Mvc;

namespace DistributedSessions.Controllers;

public class HomeController : Controller
{
    public IActionResult Index()
    {
        var books = Data.Books;

        return View(books);
    }
}