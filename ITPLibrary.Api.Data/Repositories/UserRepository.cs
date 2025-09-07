using Dapper;
using ITPLibrary.Api.Data.Entities;
using System.Data;
using System.Threading.Tasks;

namespace ITPLibrary.Api.Data.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly IDbConnection _dbConnection;

        public UserRepository(IDbConnection dbConnection)
        {
            _dbConnection = dbConnection;
        }

        public async Task<User> GetUserByEmailAndPasswordAsync(string email, string password)
        {
            var sql = "SELECT * FROM Users WHERE Email = @Email AND Password = @Password";
            return await _dbConnection.QuerySingleOrDefaultAsync<User>(sql, new { Email = email, Password = password });
        }

        public async Task<User> GetUserByEmailAsync(string email)
        {
            var sql = "SELECT * FROM Users WHERE Email = @Email";
            return await _dbConnection.QuerySingleOrDefaultAsync<User>(sql, new { Email = email });
        }

        public async Task AddUserAsync(User user)
        {
            var sql = "INSERT INTO Users (Email, Password) VALUES (@Email, @Password)";
            await _dbConnection.ExecuteAsync(sql, user);
        }
    }
}