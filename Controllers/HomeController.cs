using Microsoft.AspNetCore.Mvc;
using StackExchange.Redis;

namespace DistributedSessions.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;

    public HomeController(ILogger<HomeController> logger, IConnectionMultiplexer conn)
    {
        _logger = logger;
    }

    public IActionResult Index()
    {
        var books = Data.Books;

        return View(books);
    }
}