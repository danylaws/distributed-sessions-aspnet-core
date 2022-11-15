using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using System.Text.Json;

namespace DistributedSessions.Controllers
{
    public class CartController : Controller
    {
        private readonly IDistributedCache _cache;

        public CartController(IDistributedCache cache)
        {
            _cache = cache;
        }

        public async Task<IActionResult> Index()
        {
            // Get the value of the session
            var data = GetBooksFromSession();

            //Pass the list to the view to render
            return View(data);
        }

        public async Task<IActionResult> AddToCart(int id)
        {
            var data = await GetBooksFromRedisCache();

            var book = Data.Books.Where(b => b.Id == id).FirstOrDefault();

            if (book is not null)
            {
                data.Add(book);

                var json = JsonSerializer.Serialize(data);

                //Local in-memory session
                // HttpContext.Session.SetString("user-session-id", json);

                //Redis cache
                await _cache.SetStringAsync("user-session-id", json);

                TempData["Success"] = "The book is added successfully";

                return RedirectToAction("Index");
            }

            return NotFound();
        }

        private async Task<List<Book>> GetBooksFromRedisCache()
        {
            var cacheValue = await _cache.GetStringAsync("user-session-id");

            if (cacheValue is not null)
            {
                return JsonSerializer.Deserialize<List<Book>>(cacheValue);
            }

            return (Enumerable.Empty<Book>()).ToList();
        }

        private List<Book> GetBooksFromSession()
        {
            var sessionString = HttpContext.Session.GetString("user-session-id");

            if (sessionString is not null)
            {
                return JsonSerializer.Deserialize<List<Book>>(sessionString);
            }

            return (Enumerable.Empty<Book>()).ToList();
        }
    }
}