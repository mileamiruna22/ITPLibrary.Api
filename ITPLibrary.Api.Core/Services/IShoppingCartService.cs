using ITPLibrary.Api.Core.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ITPLibrary.Api.Core.Services
{
    public interface IShoppingCartService
    {
        Task AddShoppingCartItemAsync(int userId, int bookId);
        Task<IEnumerable<ShoppingCartItemDto>> GetShoppingCartItemsAsync(int userId);

    }
}
