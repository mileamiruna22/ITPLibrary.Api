using Dapper;
using ITPLibrary.Api.Core.Dtos;
using ITPLibrary.Api.Data.Entities;
using System.Data;

namespace ITPLibrary.Api.Core.Services
{
    public class BookService : IBookService
    {
        private readonly IDbConnection _dbConnection;

       
        public BookService(IDbConnection dbConnection)
        {
            _dbConnection = dbConnection;
        }

        public async Task<List<BookDto>> GetPopularBooksAsync()
        {
            var sql = "SELECT * FROM Books";
            var popularBooks = await _dbConnection.QueryAsync<Book>(sql);

           
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
            var sql = "INSERT INTO Books (Title, Author, Genre) VALUES (@Title, @Author, @Genre)";
            await _dbConnection.ExecuteAsync(sql, bookDto);
        }
    }
}