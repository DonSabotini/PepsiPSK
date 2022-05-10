﻿using PepsiPSK.Models.Order;
using PepsiPSK.Entities;

namespace PepsiPSK.Services.Orders
{
    public interface IOrderService
    {
        Task<List<Order>> GetOrders();
        Task<Order?> GetOrderById(Guid guid);
        Task<Order> AddOrder(Order order);
        Task<Order?> UpdateOrder(OrderDto orderDto);
        Task<string?> DeleteOrder(Guid guid);
    }
}