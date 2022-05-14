using AutoMapper;
using Microsoft.EntityFrameworkCore;
using PepsiPSK.Entities;
using PepsiPSK.Models.Order;
using PepsiPSK.Services.Users;
using PSIShoppingEngine.Data;

namespace PepsiPSK.Services.Orders
{
    public class OrderService : IOrderService
    {
        private readonly DataContext _context;
        private readonly IUserService _userService;

        public OrderService(DataContext context, IUserService userService)
        {
            _context = context;
            _userService = userService;
        }

        public async Task<List<Order>> GetOrders()
        {
            return await _context.Orders.Select(order => order).Include(t => t.Transactions).ToListAsync();
        }

        public async Task<Order?> GetOrderById(Guid guid)
        {
            var order = await _context.Orders.Include(t => t.Transactions).FirstOrDefaultAsync(o => o.Id == guid);
            return order ?? null;
        }

        public async Task<Order> AddOrder(Order order)
        {
            order.UserId = _userService.RetrieveCurrentUserId();
            await _context.Orders.AddAsync(order);
            await _context.SaveChangesAsync();
            var addedOrder = await _context.Orders.FirstOrDefaultAsync(o => o.Id == order.Id);
            return addedOrder;
        }

        public async Task<Order?> UpdateOrder(UpdateOrderDto updateOrderDto)
        {
            var order = await _context.Orders.Include(t => t.Transactions).FirstOrDefaultAsync(o => o.Id == updateOrderDto.Id);

            if (order == null)
            {
                return null;
            }

            order.Description = updateOrderDto.Description;
            await _context.SaveChangesAsync();
            return order;
        }

        public async Task<string?> DeleteOrder(Guid guid)
        {
            var order = await _context.Orders.FirstOrDefaultAsync(o => o.Id == guid);

            if (order == null)
            {
                return null;
            }

            _context.Orders.Remove(order);
            await _context.SaveChangesAsync();
            return "Successfully deleted!";
        }
    }
}
