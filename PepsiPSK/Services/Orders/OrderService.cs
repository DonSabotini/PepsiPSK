using AutoMapper;
using Microsoft.EntityFrameworkCore;
using PepsiPSK.Entities;
using PepsiPSK.Models.Order;
using PepsiPSK.Models.User;
using PepsiPSK.Utils.Authentication;
using Pepsi.Data;
using System.Security;
using PepsiPSK.Enums;


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

        public async Task<GetOrderDto> AddOrder(AddOrderDto addOrderDto)
        {
            Order newOrder = _mapper.Map<Order>(addOrderDto);
            newOrder.Description = addOrderDto.Description;
            newOrder.PaymentMethod = addOrderDto.PaymentMethod;
            newOrder.UserId = GetCurrentUserId();
            newOrder.TotalCost = 0;
            foreach (var orderedFlower in addOrderDto.FlowersForOrder)
            {
                Flower flowerFromDB = await _context.Flowers.FirstOrDefaultAsync(f => f.Id == orderedFlower.FlowerId);
                if (flowerFromDB == null)
                    throw new ArgumentException("Ordered flower with given id does not exist");
                if (flowerFromDB.NumberInStock  < orderedFlower.Amount)
                    throw new ArgumentException("Not enough flowers in stock");
                FlowerItem item = _mapper.Map<FlowerItem>(flowerFromDB);
                flowerFromDB.NumberInStock -= orderedFlower.Amount;
                item.Amount = orderedFlower.Amount;
                newOrder.Items.Add(item);
                newOrder.TotalCost += item.Price * orderedFlower.Amount;
                
            }
            await _context.Orders.AddAsync(newOrder);
            await _context.SaveChangesAsync();
            var result = _mapper.Map<GetOrderDto>(newOrder);
            User orderUser = await _context.Users.FirstOrDefaultAsync(u => u.Id == newOrder.UserId);
            result.UserInfo = _mapper.Map<UserInfoDto>(orderUser);
            return _mapper.Map<GetOrderDto>(newOrder);
        }

        
        public async Task<List<GetOrderDto>> GetOrders()
        {
            if (AdminCheck())
            {
                var ordersForAdmin = await _context.Orders.Include(x => x.Items).Select(order => order).ToListAsync();
                var result = _mapper.Map<List<GetOrderDto>>(ordersForAdmin);
                foreach (var order in result)
                {
                    User orderUser = await _context.Users.FirstOrDefaultAsync(u => u.Id == order.UserId);
                    order.UserInfo = _mapper.Map<UserInfoDto>(orderUser);
                }
                return result;
            }

            var orders = await _context.Orders.Include(x => x.Items).Where(o => o.UserId == GetCurrentUserId()).Select(order => order).ToListAsync();

            return _mapper.Map<List<GetOrderDto>>(orders);
        }
        public async Task<GetOrderDto?> GetOrderById(Guid guid)
        {
            var order = await _context.Orders.FirstOrDefaultAsync(o => o.Id == guid);
            if (order == null)
            {
                return null;
            }
            if (!AdminCheck() && order.UserId != GetCurrentUserId())
            {
                throw new SecurityException();
            }
            var result = _mapper.Map<GetOrderDto>(order);
            if (AdminCheck())
            {
                User orderUser = await _context.Users.FirstOrDefaultAsync(u => u.Id == order.UserId);
                result.UserInfo = _mapper.Map<UserInfoDto>(orderUser);
            }
            return result;            
        }
 
   

 
        public async Task<GetOrderDto?> UpdateOrder(Guid guid, ChangeOrderStatusDto updateOrderDto)
        {
            var order = await _context.Orders.Include(x => x.Items).FirstOrDefaultAsync(o => o.Id == guid);

            if (order == null)
            {
                return null;
            }

            if (!AdminCheck() && order.UserId != GetCurrentUserId())
            {
                throw new SecurityException();
            }

            if (!OrderStatusValidityCheck(updateOrderDto.OrderStatus, order))
            {
                throw new SecurityException();
            }
            order.OrderStatus = updateOrderDto.OrderStatus;
            foreach (var item in order.Items)
            {
                Flower flower = await _context.Flowers.FirstOrDefaultAsync(f => f.Id == item.FlowerId);
                if (flower != null)
                    flower.NumberInStock += item.Amount;
            }
            await _context.SaveChangesAsync();
            return _mapper.Map<GetOrderDto>(order);
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

        private bool OrderStatusValidityCheck(OrderStatus orderStatus, Order order)
        {
            bool isOrderStatusSubmitted = order.OrderStatus == OrderStatus.Submitted;
            bool isOperationValid = (orderStatus == OrderStatus.Cancelled && order.UserId == GetCurrentUserId()) 
                || (orderStatus == OrderStatus.Declined && AdminCheck());
            return isOrderStatusSubmitted && isOperationValid;
        }
    }
}
