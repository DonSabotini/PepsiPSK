﻿using PepsiPSK.Models.Order;

namespace PepsiPSK.Services.Orders
{
    public interface IOrderService
    {
        Task<List<GetOrderDto>> GetOrders();
        Task<GetOrderDto?> GetOrderById(Guid guid);
        Task<GetOrderDto> AddOrder(AddOrderDto addOrderDto);
        Task<GetOrderDto?> UpdateOrder(UpdateOrderDto updateOrderDto);
        Task<string?> DeleteOrder(Guid guid);
    }
}