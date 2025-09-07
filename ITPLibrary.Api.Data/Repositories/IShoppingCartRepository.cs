using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ITPLibrary.Api.Core.Dtos;


namespace ITPLibrary.Api.Data.Repositories
{
    public interface IShoppingCartRepository
    {
        Task AddShoppingCartItemAsync(int userId, int bookId);

        Task<IEnumerable<ShoppingCartItemDto>> GetShoppingCartItemsAsync(int userId);
    }
}