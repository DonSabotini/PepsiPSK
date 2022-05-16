using AutoMapper;
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
        private readonly IMapper _mapper;
        private readonly ICurrentUserInfoRetriever _currentUserInfoRetriever;

        public OrderService(DataContext context, IMapper mapper, ICurrentUserInfoRetriever currentUserInfoRetriever)
        {
            _context = context;
            _mapper = mapper;
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

        public async Task<List<GetOrderDto>> GetOrders()
        {
            return AdminCheck() ? await _context.Orders.Select(order => _mapper.Map<GetOrderDto>(order)).ToListAsync() : await _context.Orders.Where(o => o.UserId == GetCurrentUserId()).Select(order => _mapper.Map<GetOrderDto>(order)).ToListAsync();
        }

        public async Task<GetOrderDto?> GetOrderById(Guid guid)
        {
            var order = AdminCheck() ? await _context.Orders.FirstOrDefaultAsync(o => o.Id == guid) : await _context.Orders.FirstOrDefaultAsync(o => o.Id == guid && o.UserId == GetCurrentUserId());
            return order != null ? _mapper.Map<GetOrderDto>(order) : null;
        }

        public async Task<GetOrderDto> AddOrder(AddOrderDto addOrderDto)
        {
            Order newOrder = new();
            newOrder.Description = addOrderDto.Description;
            var flowerIds = addOrderDto.Flowers.Select(f => f.FlowerId).ToList();
            newOrder.Flowers = await _context.Flowers.Where(flower => flowerIds.Contains(flower.Id)).ToListAsync();
            newOrder.UserId = GetCurrentUserId();
            await _context.Orders.AddAsync(newOrder);
            await _context.SaveChangesAsync();

            var addedOrder = await _context.Orders.FirstOrDefaultAsync(o => o.Id == newOrder.Id);
            var flowerOrders = await _context.FlowerOrders.Where(fo => fo.OrderId == addedOrder.Id).ToListAsync();

            for (int i = 0; i < flowerOrders.Count; i++) {
                flowerOrders[i].Amount = addOrderDto.Flowers[i].Amount;
            }

            await _context.SaveChangesAsync();
            return _mapper.Map<GetOrderDto>(addedOrder);
        }

        public async Task<GetOrderDto?> UpdateOrder(UpdateOrderDto updateOrderDto)
        {
            var order = AdminCheck() ? await _context.Orders.FirstOrDefaultAsync(o => o.Id == updateOrderDto.Id) : await _context.Orders.FirstOrDefaultAsync(o => o.Id == updateOrderDto.Id && o.UserId == GetCurrentUserId());

            if (order == null)
            {
                return null;
            }

            order.Description = updateOrderDto.Description;
            order.OrderStatus = updateOrderDto.OrderStatus;
            await _context.SaveChangesAsync();
            return _mapper.Map<GetOrderDto>(order);
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
