using ITPLibrary.Api.Data.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ITPLibrary.Api.Data.Repositories
{
    public interface IBookRepository
    {
        Task<List<Book>> GetPopularBooksAsync();
        Task AddBookAsync(Book book);
        Task<List<Book>> GetPromotedBooksAsync();
        Task<List<Book>> GetBookListAsync();
        Task<Book> GetBookByIdAsync(int id);
        Task DeleteBookAsync(int id);
    }
}