using ITPLibrary.Api.Data.Entities;
using System.Threading.Tasks;

namespace ITPLibrary.Api.Data.Repositories
{
    public interface IUserRepository
    {
        Task<User> GetUserByEmailAndPasswordAsync(string email, string password);
        Task<User> GetUserByEmailAsync(string email);
        Task AddUserAsync(User user);
    }
}