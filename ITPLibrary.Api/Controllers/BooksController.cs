using ITPLibrary.Api.Core.Dtos;
using ITPLibrary.Api.Core.Services; 

using Microsoft.AspNetCore.Mvc;

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

        [HttpGet("popular")]
        public async Task<ActionResult<List<BookDto>>> GetPopularBooksAsync()
        {
            var popularBooks = await _bookService.GetPopularBooksAsync();
            return Ok(popularBooks);
        }

        [HttpPost]
        public async Task<IActionResult> AddBook([FromBody] BookDto bookDto)
        {
            await _bookService.AddBookAsync(bookDto);
            return Ok();
        }
    }
}