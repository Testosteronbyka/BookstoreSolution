using Microsoft.AspNetCore.Mvc;
using BookStore.API.Models;

namespace BookStore.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BooksController : ControllerBase
    {
        private readonly ILogger<BooksController> _logger;

        // Hardkodowane książki
        private static readonly List<Book> _books = new List<Book>
        {
            new Book 
            {
                Id = 1,
                Title = "Wiedźmin: Ostatnie życzenie",
                Author = "Andrzej Sapkowski",
                Price = 39.99m,
                Stock = 10,
                Description = "Pierwszy tom kultowej sagi o wiedźminie Geralcie z Rivii",
                ISBN = "9788375780635",
                CreatedDate = DateTime.Now
            },
            new Book 
            {
                Id = 2,
                Title = "Hobbit, czyli tam i z powrotem",
                Author = "J.R.R. Tolkien",
                Price = 49.99m,
                Stock = 5,
                Description = "Klasyczna powieść fantasy o przygodach Bilbo Bagginsa",
                ISBN = "9788324159819",
                CreatedDate = DateTime.Now
            },
            new Book 
            {
                Id = 3,
                Title = "1984",
                Author = "George Orwell",
                Price = 29.99m,
                Stock = 8,
                Description = "Dystopijny świat totalitarnej kontroli",
                ISBN = "9788382022287",
                CreatedDate = DateTime.Now
            }
        };

        public BooksController(ILogger<BooksController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public ActionResult<IEnumerable<Book>> GetAll()
        {
            _logger.LogInformation($"Zwracam {_books.Count} książek");
            return Ok(_books);
        }

        [HttpGet("{id}")]
        public ActionResult<Book> GetById(int id)
        {
            var book = _books.FirstOrDefault(b => b.Id == id);
            if (book == null)
            {
                _logger.LogWarning($"Książka o ID {id} nie została znaleziona");
                return NotFound();
            }
            
            _logger.LogInformation($"Zwracam książkę: {book.Title}");
            return Ok(book);
        }

        [HttpPost]
        public ActionResult<Book> Create(Book book)
        {
            book.Id = _books.Count > 0 ? _books.Max(b => b.Id) + 1 : 1;
            book.CreatedDate = DateTime.Now;
            _books.Add(book);
            
            _logger.LogInformation($"Dodano nową książkę: {book.Title}");
            return CreatedAtAction(nameof(GetById), new { id = book.Id }, book);
        }

        [HttpPut("{id}")]
        public IActionResult Update(int id, Book book)
        {
            var existingBook = _books.FirstOrDefault(b => b.Id == id);
            if (existingBook == null)
            {
                return NotFound();
            }

            existingBook.Title = book.Title;
            existingBook.Author = book.Author;
            existingBook.Price = book.Price;
            existingBook.Stock = book.Stock;
            existingBook.Description = book.Description;
            existingBook.ISBN = book.ISBN;

            _logger.LogInformation($"Zaktualizowano książkę: {existingBook.Title}");
            return NoContent();
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var book = _books.FirstOrDefault(b => b.Id == id);
            if (book == null)
            {
                return NotFound();
            }

            _books.Remove(book);
            _logger.LogInformation($"Usunięto książkę: {book.Title}");
            return NoContent();
        }
    }
}
