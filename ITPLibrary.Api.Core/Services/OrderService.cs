using ITPLibrary.Api.Core.Dtos;
using ITPLibrary.Api.Data.Repositories;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;

namespace ITPLibrary.Api.Core.Services
{
    public class OrderService : IOrderService
    {
        private readonly IOrderRepository _orderRepository;

        public OrderService(IOrderRepository orderRepository)
        {
            _orderRepository = orderRepository;
        }

        public async Task<int> Checkout(int userId, PlaceOrderDto placeOrderDto)
        {
            return await _orderRepository.Checkout(
                userId,
                placeOrderDto.ShippingAddress.Street,
                placeOrderDto.ShippingAddress.City,
                placeOrderDto.ShippingAddress.State,
                placeOrderDto.ShippingAddress.PostalCode,
                placeOrderDto.ShippingAddress.Country,
                placeOrderDto.BookIds
            );
        }

        public async Task<List<OrderDto>> GetUserOrders(int userId)
        {
            var orders = await _orderRepository.GetUserOrders(userId);

            var ordersDto = new List<OrderDto>();

            foreach (var order in orders)
            {
                var orderDto = new OrderDto
                {
                    Id = order.Id,
                    TotalAmount = order.TotalAmount,
                    OrderDate = order.OrderDate,
                    ShippingAddress = new AddressDto
                    {
                        Street = order.Street,
                        City = order.City,
                        State = order.State,
                        PostalCode = order.PostalCode,
                        Country = order.Country,
                    },
                    OrderItems = new List<OrderItemDto>()
                };

                foreach (var item in order.OrderItems)
                {
                    orderDto.OrderItems.Add(new OrderItemDto
                    {
                        BookId = item.BookId,
                        Title = item.Book.Title,
                        Author = item.Book.Author,
                        Price = item.Book.Price
                    });
                }
                ordersDto.Add(orderDto);
            }

            return ordersDto;
        }

        public async Task UpdateOrderStatus(int userId, UpdateOrderStatusDto updateOrderStatusDto)
        {
            var order = await _orderRepository.GetOrderById(updateOrderStatusDto.OrderId);

            if (order.UserId != userId)
            {
                throw new UnauthorizedAccessException("You are not authorized to update this order.");
            }

            if (order.Status == "Delivered" || order.Status == "Canceled")
            {
                throw new InvalidOperationException($"Order with ID {order.Id} cannot be updated because its current status is '{order.Status}'.");
            }

            await _orderRepository.UpdateOrderStatus(updateOrderStatusDto.OrderId, updateOrderStatusDto.NewStatus);
        }
    }
}