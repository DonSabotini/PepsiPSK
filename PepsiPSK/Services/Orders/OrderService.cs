using Microsoft.EntityFrameworkCore;
using PepsiPSK.Entities;
using PepsiPSK.Models.Order;
using PSIShoppingEngine.Data;

namespace PepsiPSK.Services.Orders
{
    public class OrderService : IOrderService
    {
        private readonly DataContext _context;

        public OrderService(DataContext context)
        {
            _context = context;
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
            await _context.Orders.AddAsync(order);
            await _context.SaveChangesAsync();
            var addedOrder = await _context.Orders.FirstOrDefaultAsync(o => o.Id == order.Id);
            return addedOrder;
        }

        public async Task<Order?> UpdateOrder(OrderDto orderDto)
        {
            var order = await _context.Orders.Include(t => t.Transactions).FirstOrDefaultAsync(o => o.Id == orderDto.Id);

            if (order == null)
            {
                return null;
            }

            order.Description = orderDto.Description;
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
