using ITPLibrary.Api.Core.Dtos;
using System.Threading.Tasks;

namespace ITPLibrary.Api.Core.Services
{
    public interface IOrderService
    {
        Task<int> Checkout(int userId, PlaceOrderDto placeOrderDto);
        Task<List<OrderDto>> GetUserOrders(int userId);
        Task UpdateOrderStatus(int userId, UpdateOrderStatusDto updateOrderStatusDto);
    }
}