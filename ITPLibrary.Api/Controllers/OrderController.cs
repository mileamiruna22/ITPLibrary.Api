using ITPLibrary.Api.Core.Dtos;
using ITPLibrary.Api.Core.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace ITPLibrary.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly IOrderService _orderService;

        public OrderController(IOrderService orderService)
        {
            _orderService = orderService;
        }

        [HttpPost("checkout")]
        [Authorize]
        public async Task<IActionResult> Checkout1([FromBody] PlaceOrderDto placeOrderDto)
        {
            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));

            var orderId = await _orderService.Checkout1(userId, placeOrderDto);

            return Ok(new { OrderId = orderId });
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetUserOrders()
        {
            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));

            var orders = await _orderService.GetUserOrders(userId);


            return Ok(orders);
        }


        [HttpGet("{id}")]
        [Authorize]
        public async Task<IActionResult> GetOrderById(int id)
        {
            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));

            var order = await _orderService.GetOrderById(userId, id);

            if (order == null)
            {
                return NotFound(new { message = "Order not found" });
            }

            return Ok(order);
        }


        [HttpPut("{id}/details")]
        [Authorize]
        public async Task<IActionResult> UpdateOrderDetails(int id, [FromBody] UpdateOrderDetailsDto updateDto)
        {
            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));

            await _orderService.UpdateOrderDetails(userId, id, updateDto);

            return Ok(new { message = "Order updated successfully" });
        }

        [HttpPut]
        [Authorize]
        public async Task<IActionResult> UpdateOrderStatus([FromBody] UpdateOrderStatusDto updateOrderStatusDto)
        {
            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));

            await _orderService.UpdateOrderStatus(userId, updateOrderStatusDto);

            return Ok();
        }
    }
}