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
            // Aici va veni logica de afaceri
        }
    }
}