using ITPLibrary.Api.Core.Dtos;
using ITPLibrary.Api.Core.Services;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace ITPLibrary.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BooksController : ControllerBase
    {
        private readonly IBookService _bookService; 

        public BooksController(IBookService bookService)
        {
            _bookService = bookService;
        }

        [HttpGet("promoted")]
        public async Task<IActionResult> GetPromotedBooks()
        {
            var books = await _bookService.GetPromotedBooksAsync();
            return Ok(books);
        }

        [HttpGet("popular")]
        public async Task<IActionResult> GetPopularBooks()
        {
            var books = await _bookService.GetPopularBooksAsync();
            return Ok(books);
        }

        [HttpGet("all")]
        public async Task<IActionResult> GetBookList()
        {
            var books = await _bookService.GetBookListAsync();
            return Ok(books);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetBookDetails([FromRoute] int id)
        {
            var book = await _bookService.GetBookDetailsAsync(id);

            if (book == null)
            {
                return NotFound();
            }

            return Ok(book);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBook([FromRoute] int id)
        {
            await _bookService.DeleteBookAsync(id);
            return Ok("Book deleted successfully.");
        }

        [HttpPost]
        public async Task<IActionResult> AddBook([FromBody] BookDto bookDto)
        {
            await _bookService.AddBookAsync(bookDto);
            return Ok("Book added successfully.");
        }
    }
}