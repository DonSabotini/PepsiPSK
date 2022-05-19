using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PepsiPSK.Models.Order;
using PepsiPSK.Services.Orders;

namespace PepsiPSK.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class OrdersController : ControllerBase
    {
        private readonly IOrderService _orderService;

        public OrdersController(IOrderService orderService)
        {
            _orderService = orderService;
        }

        [Authorize(Roles = "User, Admin")]
        [HttpGet]
        public async Task<IActionResult> GetOrders()
        {
            var orders = await _orderService.GetOrders();
            return Ok(orders);
        }

        [Authorize(Roles = "User, Admin")]
        [HttpGet("{guid}")]
        public async Task<IActionResult> GetOrderById(Guid guid)
        {
            var order = await _orderService.GetOrderById(guid);
            return order == null ? NotFound() : Ok(order);
        }

        [Authorize(Roles = "User, Admin")]
        [HttpPost]
        public async Task<IActionResult> AddOrder(AddOrderDto addOrderDto)
        {
            var addedOrder = await _orderService.AddOrder(addOrderDto);
            return Ok(addedOrder);
        }

        [Authorize(Roles = "User, Admin")]
        [HttpPut]
        public async Task<IActionResult> UpdateOrder(UpdateOrderDto updateOrderDto)
        {
            var order = await _orderService.UpdateOrder(updateOrderDto);
            return order == null ? NotFound() : Ok(order);
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("{guid}")]
        public async Task<IActionResult> DeleteOrder(Guid guid)
        {
            var successMessage = await _orderService.DeleteOrder(guid);
            return successMessage == null ? NotFound() : Ok(successMessage);
        }
    }
}
