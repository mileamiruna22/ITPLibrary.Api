using Dapper;
//using ITPLibrary.Api.Core.Dtos; 
using ITPLibrary.Api.Data.Models;
using ITPLibrary.Api.Data.Repositories;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System.Data;
using System.Threading.Tasks;

namespace ITPLibrary.Api.Data.Repositories
{
    public class ShoppingCartRepository : IShoppingCartRepository
    {
        private readonly IDbConnection _dbConnection;

        public ShoppingCartRepository(IConfiguration configuration)
        {
            _dbConnection = new SqlConnection(configuration.GetConnectionString("DefaultConnection"));
        }

        public async Task AddShoppingCartItemAsync(int userId, int bookId)
        {
            var sql = "INSERT INTO ShoppingCart (UserId, BookId) VALUES (@UserId, @BookId)";
            await _dbConnection.ExecuteAsync(sql, new { UserId = userId, BookId = bookId });
        }

        public async Task<IEnumerable<ShoppingCartItem>> GetShoppingCartItemsAsync(int userId)
        {
            var sql = @"
        SELECT
            sc.Id,
            sc.BookId,
            b.Title,
            b.Price,
            b.Author,
            b.Thumbnail
        FROM
            ShoppingCart sc
        JOIN
            Books b ON sc.BookId = b.Id
        WHERE
            sc.UserId = @UserId";
            return await _dbConnection.QueryAsync<ShoppingCartItem>(sql, new { UserId = userId });
        }
    }
}