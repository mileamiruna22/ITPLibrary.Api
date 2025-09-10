using Dapper;
using ITPLibrary.Api.Data.Entities;
using ITPLibrary.Api.Data.Models;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace ITPLibrary.Api.Data.Repositories
{
    public class OrderRepository : IOrderRepository
    {
        private readonly IDbConnection _dbConnection;

        public OrderRepository(IConfiguration configuration)
        {
            _dbConnection = new SqlConnection(configuration.GetConnectionString("DefaultConnection"));
        }

        public async Task<int> Checkout(int userId, string street, string city, string state, string postalCode, string country, List<int> bookIds)
        {
            var sql = "spCheckout";
            var parameters = new DynamicParameters();
            parameters.Add("@UserId", userId);
            parameters.Add("@BookIds", string.Join(",", bookIds));
            parameters.Add("@Street", street);
            parameters.Add("@City", city);
            parameters.Add("@State", state);
            parameters.Add("@PostalCode", postalCode);
            parameters.Add("@Country", country);
            parameters.Add("@OrderId", dbType: DbType.Int32, direction: ParameterDirection.Output);

            await _dbConnection.ExecuteAsync(sql, parameters, commandType: CommandType.StoredProcedure);

            return parameters.Get<int>("@OrderId");
        }

        public async Task<List<Order>> GetUserOrders(int userId)
        {
            var sql = @"
        SELECT 
            o.Id, o.UserId, o.TotalAmount, o.OrderDate,
            o.Street, o.City, o.State, o.PostalCode, o.Country,
            oi.OrderId, oi.BookId,
            b.Id, b.Title, b.Author, b.Description, b.Price, b.ImageUrl, b.Category
        FROM Orders o
        JOIN OrderItems oi ON o.Id = oi.OrderId
        JOIN Books b ON oi.BookId = b.Id
        WHERE o.UserId = @UserId";

            var orderDictionary = new Dictionary<int, Order>();

            var orders = await _dbConnection.QueryAsync<Order, OrderItem, Book, Order>(
                sql,
                (order, orderItem, book) =>
                {
                    if (!orderDictionary.TryGetValue(order.Id, out var existingOrder))
                    {
                        existingOrder = order;
                        existingOrder.OrderItems = new List<OrderItem>();
                        orderDictionary.Add(existingOrder.Id, existingOrder);
                    }

                    if (orderItem != null)
                    {
                        orderItem.Book = book;
                        existingOrder.OrderItems.Add(orderItem);
                    }

                    return existingOrder;
                },
                new { UserId = userId },
                splitOn: "OrderId, Id"
            );

            return orders.Distinct().ToList();
        }

        public async Task UpdateOrderStatus(int orderId, string newStatus)
        {
            var sql = "UPDATE Orders SET Status = @NewStatus WHERE Id = @OrderId";
            await _dbConnection.ExecuteAsync(sql, new { NewStatus = newStatus, OrderId = orderId });
        }

        public async Task<Order> GetOrderById(int orderId)
        {
            var sql = "SELECT * FROM Orders WHERE Id = @OrderId";
            var order = await _dbConnection.QuerySingleOrDefaultAsync<Order>(sql, new { OrderId = orderId });
            return order;
        }
    }
}