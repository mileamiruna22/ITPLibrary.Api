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
        public async Task<IActionResult> Checkout([FromBody] PlaceOrderDto placeOrderDto)
        {
            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));

            var orderId = await _orderService.Checkout(userId, placeOrderDto);

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