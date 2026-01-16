using ITPLibrary.Api.Data.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ITPLibrary.Api.Data.Repositories
{
    public interface IOrderRepository
    {
        Task<int> Checkout1(int userId, string street, string city, string state, string postalCode, string country, List<int> bookIds);
        Task<List<Order>> GetUserOrders(int userId);
        Task UpdateOrderStatus(int orderId, string newStatus);
        Task<Order> GetOrderById(int orderId);
        Task UpdateOrderDetails(
             int orderId,
             string street,
             string city,
             string state,
             string postalCode,
             string country
         );
    }
}