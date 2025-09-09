using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
//using ITPLibrary.Api.Core.Dtos;
using ITPLibrary.Api.Data.Models;



namespace ITPLibrary.Api.Data.Repositories
{
    public interface IShoppingCartRepository
    {
        Task AddShoppingCartItemAsync(int userId, int bookId);

        Task<IEnumerable<ShoppingCartItem>> GetShoppingCartItemsAsync(int userId);
    }
}