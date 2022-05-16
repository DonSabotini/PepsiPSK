using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PepsiPSK.Models.Order;
using PepsiPSK.Services.Orders;

namespace PepsiPSK.Controllers
{
    [Authorize(Roles = "User, Admin")]
    [ApiController]
    [Route("[controller]")]
    public class OrdersController : ControllerBase
    {
        private readonly IOrderService _orderService;

        public OrdersController(IOrderService orderService)
        {
            _orderService = orderService;
        }

        [HttpGet]
        public async Task<IActionResult> GetOrders()
        {
            var orders = await _orderService.GetOrders();
            return Ok(orders);
        }

        [HttpGet("{guid}")]
        public async Task<IActionResult> GetOrderById(Guid guid)
        {
            var order = await _orderService.GetOrderById(guid);
            return order == null ? NotFound() : Ok(order);
        }

        [HttpPost]
        public async Task<IActionResult> AddOrder(AddOrderDto addOrderDto)
        {
            var addedOrder = await _orderService.AddOrder(addOrderDto);
            return Ok(addedOrder);
        }

        [HttpPut]
        public async Task<IActionResult> UpdateOrder(UpdateOrderDto updateOrderDto)
        {
            var order = await _orderService.UpdateOrder(updateOrderDto);
            return order == null ? NotFound() : Ok(order);
        }

        [HttpDelete("{guid}")]
        public async Task<IActionResult> DeleteOrder(Guid guid)
        {
            var successMessage = await _orderService.DeleteOrder(guid);
            return successMessage == null ? NotFound() : Ok(successMessage);
        }
    }
}
