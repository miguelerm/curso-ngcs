using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Abs.BooksCatalog.Service.Data;
using Microsoft.Extensions.Logging;
using MassTransit;
using Abs.Messages.BooksCatalog.Events;

namespace Abs.BooksCatalog.Service.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BooksController : ControllerBase
    {
        private readonly BooksCatalogContext context;
        private readonly ILogger<BooksController> logger;
        private readonly IBus bus;

        public BooksController(BooksCatalogContext context, ILogger<BooksController> logger, IBus bus)
        {
            this.logger = logger;
            this.bus = bus;
            this.context = context;
        }

        // GET: api/Books
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Book>>> GetBooks()
        {
            logger.LogDebug("Getting all books");
            return await context.Books.Include(x => x.Authors).Include(x => x.Covers).ToListAsync();
        }

        // GET: api/Books/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Book>> GetBook(int id)
        {
            var book = await context.Books.Include(x => x.Authors).Include(x => x.Covers).FirstOrDefaultAsync(x => x.Id == id);

            if (book == null)
            {
                logger.LogWarning("Book with id {id} not found", id);
                return NotFound();
            }

            return book;
        }

        // PUT: api/Books/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutBook(int id, Book book)
        {
            if (id != book.Id)
            {
                logger.LogWarning("Trying to edit book {@book} specifying id {id}", book, id);
                return BadRequest();
            }

            context.Entry(book).State = EntityState.Modified;

            try
            {
                await context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException ex)
            {
                if (!BookExists(id))
                {
                    logger.LogWarning("Book with id {id} not found", id);
                    return NotFound();
                }
                else
                {
                    logger.LogError(ex, "Error trying to update book {@book} with id {id}", book, id);
                    throw;
                }
            }

            logger.LogDebug("Book Saved {@book}", book);
            return NoContent();
        }

        // POST: api/Books
        [HttpPost]
        public async Task<ActionResult<Book>> PostBook(Book book)
        {
            context.Books.Add(book);
            await context.SaveChangesAsync();
            await bus.Publish<IBookCreated>(book, c => {
                c.CorrelationId = NewId.NextGuid();
                c.Headers.Set("_user", "fulanito");
            });

            return CreatedAtAction("GetBook", new { id = book.Id }, book);
        }

        // DELETE: api/Books/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Book>> DeleteBook(int id)
        {
            var book = await context.Books.FindAsync(id);
            if (book == null)
            {
                return NotFound();
            }

            context.Books.Remove(book);
            await context.SaveChangesAsync();

            return book;
        }

        private bool BookExists(int id)
        {
            return context.Books.Any(e => e.Id == id);
        }
    }
}
