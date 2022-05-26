using PepsiPSK.Models.Order;
using PepsiPSK.Responses.Service;

namespace PepsiPSK.Services.Orders
{
    public interface IOrderService
    {
        Task<List<GetOrderDto>> GetOrders();
        Task<ServiceResponse<GetOrderDto?>> GetOrderById(Guid guid);
        Task<GetOrderDto> AddOrder(AddOrderDto addOrderDto);
        Task<ServiceResponse<GetOrderDto?>> UpdateOrder(Guid guid, ChangeOrderStatusDto updateOrderDto);
        Task<string?> DeleteOrder(Guid guid);
    }
}