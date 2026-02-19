using ITPLibrary.Api.Core.Dtos;

namespace ITPLibrary.Api.Core.Services
{
    public interface IBookService
    {
        Task<List<BookDto>> GetPopularBooksAsync();
        Task AddBookAsync(BookDto bookDto);
        Task<List<BookDto>> GetPromotedBooksAsync();
        //Task<List<BookListDto>> GetBookListAsync();
        Task<List<BookListDto>> GetBookListAsync(int page, int pageSize);
        Task<BookDetailsDto> GetBookDetailsAsync(int id);
        Task DeleteBookAsync(int id);
    }
}