using ITPLibrary.Api.Core.Dtos;
using ITPLibrary.Api.Data.Entities;
using ITPLibrary.Api.Data.Repositories;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ITPLibrary.Api.Core.Services
{
    public class BookService : IBookService
    {
        private readonly IBookRepository _bookRepository;

        public BookService(IBookRepository bookRepository)
        {
            _bookRepository = bookRepository;
        }

        public async Task<List<BookDto>> GetPopularBooksAsync()
        {
            var popularBooks = await _bookRepository.GetPopularBooksAsync();

            var bookDtos = new List<BookDto>();
            foreach (var book in popularBooks)
            {
                bookDtos.Add(new BookDto
                {
                    Id = book.Id,
                    Title = book.Title,
                    Author = book.Author,
                    Genre = book.Genre
                });
            }

            return bookDtos;
        }

        public async Task AddBookAsync(BookDto bookDto)
        {
            var book = new Book
            {
                Title = bookDto.Title,
                Author = bookDto.Author,
                Genre = bookDto.Genre
            };

            await _bookRepository.AddBookAsync(book);
        }

        public async Task<List<BookListDto>> GetBookListAsync()
        {
            var books = await _bookRepository.GetBookListAsync();

            var bookListDtos = new List<BookListDto>();
            foreach (var book in books)
            {
                bookListDtos.Add(new BookListDto
                {
                    Id = book.Id,
                    Title = book.Title,
                    Price = book.Price,
                    Author = book.Author,
                    Thumbnail = book.Thumbnail,
                    Popular = book.Popular,
                    RecentlyAdded = book.RecentlyAdded
                });
            }

            return bookListDtos;
        }

        public async Task<List<BookDto>> GetPromotedBooksAsync()
        {
            var promotedBooks = await _bookRepository.GetPromotedBooksAsync();

            var bookDtos = new List<BookDto>();
            foreach (var book in promotedBooks)
            {
                bookDtos.Add(new BookDto
                {
                    Id = book.Id,
                    Title = book.Title,
                    ShortDescription = book.ShortDescription,
                    ImageUrl = book.ImageUrl
                });
            }
            return bookDtos;
        }

        public async Task<BookDetailsDto> GetBookDetailsAsync(int id)
        {
            var book = await _bookRepository.GetBookByIdAsync(id);

            if (book == null)
            {
                return null;
            }

       
            var bookDetailsDto = new BookDetailsDto
            {
                Id = book.Id,
                Title = book.Title,
                Price = book.Price,
                Author = book.Author,
                LongDescription = book.LongDescription, 
                Image = book.Image 
            };

            return bookDetailsDto;
        }

        public async Task DeleteBookAsync(int id)
        {
            var book = await _bookRepository.GetBookByIdAsync(id);

            //if (book == null)
            //{
                
            //    return;
            //}

            await _bookRepository.DeleteBookAsync(id);
        }
    }
}