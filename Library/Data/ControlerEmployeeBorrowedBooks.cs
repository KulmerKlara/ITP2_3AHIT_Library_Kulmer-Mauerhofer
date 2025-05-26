using Microsoft.AspNetCore.Mvc;
using Library.Data;  // Namespace mit deinem BookRepository

namespace Library.Web.Controllers
{
    [Route("employee")]
    public class EmployeeController : Controller
    {
        private readonly BookRepository _bookRepo;

        public EmployeeController()
        {
            _bookRepo = new BookRepository();
        }

        // API-Endpunkt: Gibt JSON aller ausgeliehenen Bücher zurück
        [HttpGet("borrowed-books")]
        public IActionResult GetBorrowedBooks()
        {
            var borrowedBooks = _bookRepo.GetBorrowedBooks();

            // Falls du nur verfügbare == false meinst, kannst du auch filtern
            // borrowedBooks = borrowedBooks.Where(b => !b.IsAvailable).ToList();

            return Json(borrowedBooks.Select(b => new
            {
                title = b.Title,
                author = b.Author,
                genre = b.Genre,
                giveBackDate = b.GiveBackDate?.ToString("yyyy-MM-dd")
            }));
        }

        // Optional: Die HTML-Seite unter /employee/borrowed-books anzeigen
        [HttpGet("borrowed-books/page")]
        public IActionResult BorrowedBooksPage()
        {
            return View();
        }
    }
}
