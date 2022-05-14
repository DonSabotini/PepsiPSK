using Microsoft.EntityFrameworkCore;
using PepsiPSK.Entities;
using PepsiPSK.Models.Order;
using PepsiPSK.Utils.Authentication;
using PSIShoppingEngine.Data;

namespace PepsiPSK.Services.Orders
{
    public class OrderService : IOrderService
    {
        private readonly DataContext _context;
        private readonly ICurrentUserInfoRetriever _currentUserInfoRetriever;

        public OrderService(DataContext context, ICurrentUserInfoRetriever currentUserInfoRetriever)
        {
            _context = context;
            _currentUserInfoRetriever = currentUserInfoRetriever;
        }

        private string GetCurrentUserId()
        {
            return _currentUserInfoRetriever.RetrieveCurrentUserId();
        }

        private bool AdminCheck()
        {
            return _currentUserInfoRetriever.CheckIfCurrentUserIsAdmin();
        }

        public async Task<List<Order>> GetOrders()
        {
            return AdminCheck() ? await _context.Orders.Include(t => t.Transactions).ToListAsync() : await _context.Orders.Include(t => t.Transactions).Where(o => o.UserId == GetCurrentUserId()).ToListAsync();
        }

        public async Task<Order?> GetOrderById(Guid guid)
        {
            var order = AdminCheck() ? await _context.Orders.Include(t => t.Transactions).FirstOrDefaultAsync(o => o.Id == guid) : await _context.Orders.Include(t => t.Transactions).FirstOrDefaultAsync(o => o.Id == guid && o.UserId == GetCurrentUserId());
            return order ?? null;
        }

        public async Task<Order> AddOrder(Order order)
        {
            order.UserId = GetCurrentUserId();
            await _context.Orders.AddAsync(order);
            await _context.SaveChangesAsync();
            var addedOrder = await _context.Orders.FirstOrDefaultAsync(o => o.Id == order.Id);
            return addedOrder;
        }

        public async Task<Order?> UpdateOrder(UpdateOrderDto updateOrderDto)
        {
            var order = AdminCheck() ? await _context.Orders.Include(t => t.Transactions).FirstOrDefaultAsync(o => o.Id == updateOrderDto.Id) : await _context.Orders.Include(t => t.Transactions).FirstOrDefaultAsync(o => o.Id == updateOrderDto.Id && o.UserId == GetCurrentUserId());

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
            var order = AdminCheck() ? await _context.Orders.FirstOrDefaultAsync(o => o.Id == guid) : await _context.Orders.FirstOrDefaultAsync(o => o.Id == guid && o.UserId == GetCurrentUserId());

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
