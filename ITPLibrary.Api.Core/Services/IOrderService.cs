using ITPLibrary.Api.Core.Dtos;
using System.Threading.Tasks;

namespace ITPLibrary.Api.Core.Services
{
    public interface IOrderService
    {
        Task<int> Checkout1(int userId, PlaceOrderDto placeOrderDto);
        Task<List<OrderDto>> GetUserOrders(int userId);
        Task<OrderDto> GetOrderById(int userId, int orderId);
        Task UpdateOrderStatus(int userId, UpdateOrderStatusDto updateOrderStatusDto);

        Task UpdateOrderDetails(int userId, int orderId, UpdateOrderDetailsDto updateDto);
    }
}