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

        public async Task<int> Checkout1(int userId, PlaceOrderDto placeOrderDto)
        {
            return await _orderRepository.Checkout1(
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
                    Status = order.Status,
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
                        Quantity = item.Quantity, 
                        PricePerUnit = item.PricePerUnit
                    });
                }
                ordersDto.Add(orderDto);
            }

            return ordersDto;
        }

        public async Task<OrderDto> GetOrderById(int userId, int orderId)
        {
            var order = await _orderRepository.GetOrderById(orderId);

            if (order == null || order.UserId != userId)
            {
                return null;
            }

            var orderDto = new OrderDto
            {
                Id = order.Id,
                TotalAmount = order.TotalAmount,
                Status = order.Status,
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
                    Title = item.Book?.Title,
                    Author = item.Book?.Author,
                    Quantity = item.Quantity,
                    PricePerUnit = item.PricePerUnit
                });
            }

            return orderDto;
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

        public async Task UpdateOrderDetails(int userId, int orderId, UpdateOrderDetailsDto updateDto)
        {
            var order = await _orderRepository.GetOrderById(orderId);

            if (order == null || order.UserId != userId)
            {
                throw new UnauthorizedAccessException("You are not authorized to update this order.");
            }

            if (order.Status == "Completed" || order.Status == "Canceled")
            {
                throw new InvalidOperationException($"Order with ID {orderId} cannot be updated because its status is '{order.Status}'.");
            }

            await _orderRepository.UpdateOrderDetails(
                orderId,
                updateDto.Street,
                updateDto.City,
                updateDto.State,
                updateDto.PostalCode,
                updateDto.Country
            );
        }
    }
}