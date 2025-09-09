using ITPLibrary.Api.Data.Repositories;
using System.Threading.Tasks;
using System.Collections.Generic;
using ITPLibrary.Api.Core.Dtos;

namespace ITPLibrary.Api.Core.Services
{
    public class ShoppingCartService : IShoppingCartService
    {
        private readonly IShoppingCartRepository _shoppingCartRepository;

        public ShoppingCartService(IShoppingCartRepository shoppingCartRepository)
        {
            _shoppingCartRepository = shoppingCartRepository;
        }

        public async Task AddShoppingCartItemAsync(int userId, int bookId)
        {
            await _shoppingCartRepository.AddShoppingCartItemAsync(userId, bookId);
        }

        public async Task<IEnumerable<ShoppingCartItemDto>> GetShoppingCartItemsAsync(int userId)
        {
            
            var items = await _shoppingCartRepository.GetShoppingCartItemsAsync(userId);
            return items.Select(item => new ShoppingCartItemDto
            {
                Id = item.Id,
                BookId = item.BookId,
                Title = item.Title,
                Price = item.Price,
                Author = item.Author,
                Thumbnail = item.Thumbnail
            });
        }
    }
}