using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using System.Text.Json;

namespace DistributedSessions.Controllers
{
    public class CartController : Controller
    {
        public async Task<IActionResult> Index()
        {
            // Get the value of the session
            var data = await GetBooksFromSession();

            //Pass the list to the view to render
            return View(data);
        }

        public async Task<IActionResult> AddToCart(int id)
        {
            var data = await GetBooksFromSession();

            var book = Data.Books.FirstOrDefault(b => b.Id == id);

            if (book is not null)
            {
                data.Add(book);

                var json = JsonSerializer.Serialize(data);

                //Local in-memory session
                HttpContext.Session.SetString("cart", json);

                TempData["Success"] = "The book is added successfully";

                return RedirectToAction("Index");
            }

            return NotFound();
        }

        private async Task<List<Book>> GetBooksFromSession()
        {
            await HttpContext.Session.LoadAsync();

            var sessionString = HttpContext.Session.GetString("cart");

            if (sessionString is not null)
            {
                return JsonSerializer.Deserialize<List<Book>>(sessionString);
            }

            return (Enumerable.Empty<Book>()).ToList();
        }
    }
}