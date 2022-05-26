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
            var serviceResponse = await _orderService.GetOrderById(guid);

            if (serviceResponse.StatusCode == 404)
            {
                return StatusCode(serviceResponse.StatusCode, serviceResponse);
            }

            if (serviceResponse.StatusCode == 401)
            {
                return StatusCode(serviceResponse.StatusCode, serviceResponse);
            }

            return StatusCode(serviceResponse.StatusCode, serviceResponse);
        }

        [Authorize(Roles = "User, Admin")]
        [HttpPost]
        public async Task<IActionResult> AddOrder(AddOrderDto addOrderDto)
        {
            var addedOrder = await _orderService.AddOrder(addOrderDto);
            return Ok(addedOrder);
        }

        [Authorize(Roles = "User, Admin")]
        [HttpPut("{guid}/change-order-status")]
        public async Task<IActionResult> UpdateOrder(Guid guid, ChangeOrderStatusDto changeOrderStatusDto)
        {
            var serviceResponse = await _orderService.UpdateOrder(guid, changeOrderStatusDto);

            if (serviceResponse.StatusCode == 404)
            {
                return StatusCode(serviceResponse.StatusCode, serviceResponse);
            }

            if (serviceResponse.StatusCode == 401)
            {
                return StatusCode(serviceResponse.StatusCode, serviceResponse);
            }

            if (serviceResponse.StatusCode == 400)
            {
                return StatusCode(serviceResponse.StatusCode, serviceResponse);
            }

            return StatusCode(serviceResponse.StatusCode, serviceResponse);
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
