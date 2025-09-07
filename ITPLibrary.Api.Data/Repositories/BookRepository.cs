using Dapper;
using ITPLibrary.Api.Data.Entities;
using System.Data;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ITPLibrary.Api.Data.Repositories
{
    public class BookRepository : IBookRepository
    {
        private readonly IDbConnection _dbConnection;

        public BookRepository(IDbConnection dbConnection)
        {
            _dbConnection = dbConnection;
        }

        public async Task<List<Book>> GetPopularBooksAsync()
        {
            var sql = "SELECT * FROM Books";
            return (await _dbConnection.QueryAsync<Book>(sql)).AsList();
        }

        public async Task AddBookAsync(Book book)
        {
            var sql = "INSERT INTO Books (Title, Author, Genre) VALUES (@Title, @Author, @Genre)";
            await _dbConnection.ExecuteAsync(sql, book);
        }

        public async Task<List<Book>> GetPromotedBooksAsync()
        {
            var sql = "SELECT * FROM Books WHERE IsPromoted = 1";
            return (await _dbConnection.QueryAsync<Book>(sql)).AsList();
        }

        public async Task<List<Book>> GetBookListAsync()
        {
            var sql = "SELECT * FROM Books";
            return (await _dbConnection.QueryAsync<Book>(sql)).AsList();
        }

        public async Task<Book> GetBookByIdAsync(int id)
        {
            var sql = "SELECT * FROM Books WHERE Id = @Id";
            return await _dbConnection.QuerySingleOrDefaultAsync<Book>(sql, new { Id = id });
        }

        public async Task DeleteBookAsync(int id)
        {
            var sql = "DELETE FROM Books WHERE Id = @Id";
            await _dbConnection.ExecuteAsync(sql, new { Id = id });
        }
    }
}