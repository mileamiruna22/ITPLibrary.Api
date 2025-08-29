using ITPLibrary.Api.Core.Dtos;

namespace ITPLibrary.Api.Core.Services
{
    public interface IBookService
    {
        Task<List<BookDto>> GetPopularBooksAsync();
        Task AddBookAsync(BookDto bookDto);
    }
}