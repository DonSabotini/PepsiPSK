using AutoMapper;
using Microsoft.EntityFrameworkCore;
using PepsiPSK.Entities;
using PepsiPSK.Models.Order;
using PepsiPSK.Models.User;
using PepsiPSK.Utils.Authentication;
using Pepsi.Data;
using PepsiPSK.Enums;
using PepsiPSK.Responses.Service;

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

                if (flowerFromDB != null)
                {
                    if (flowerFromDB.NumberInStock >= orderedFlower.Amount)
                    {
                        FlowerItem item = _mapper.Map<FlowerItem>(flowerFromDB);
                        flowerFromDB.NumberInStock -= orderedFlower.Amount;
                        item.Amount = orderedFlower.Amount;
                        newOrder.Items.Add(item);
                        newOrder.TotalCost += item.Price * orderedFlower.Amount;
                    }
                }                  
            }

            await _context.Orders.AddAsync(newOrder);
            await _context.SaveChangesAsync();
            var result = _mapper.Map<GetOrderDto>(newOrder);
            User orderUser = await _context.Users.FirstOrDefaultAsync(u => u.Id == newOrder.UserId);
            result.UserInfo = _mapper.Map<UserInfoDto>(orderUser);
            return result;
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

            var orders = await _context.Orders.Where(o => o.UserId == GetCurrentUserId()).Select(order => order).ToListAsync();

            return _mapper.Map<List<GetOrderDto>>(orders);
        }

        public async Task<ServiceResponse<GetOrderDto?>> GetOrderById(Guid guid)
        {
            var order = await _context.Orders.FirstOrDefaultAsync(o => o.Id == guid);
            var serviceResponse = new ServiceResponse<GetOrderDto?>();

            if (order == null)
            {
                serviceResponse.Data = null;
                serviceResponse.StatusCode = 404;
                serviceResponse.Message = "Specified order was not found!";

                return serviceResponse;
            }

            if (!AdminCheck() && order.UserId != GetCurrentUserId())
            {
                serviceResponse.Data = null;
                serviceResponse.StatusCode = 401;
                serviceResponse.Message = "You do not have permission to view this order!";

                return serviceResponse;
            }

            var result = _mapper.Map<GetOrderDto>(order);
            User orderUser = await _context.Users.FirstOrDefaultAsync(u => u.Id == order.UserId);
            result.UserInfo = _mapper.Map<UserInfoDto>(orderUser);

            serviceResponse.Data = result;
            serviceResponse.StatusCode = 200;
            serviceResponse.IsSuccessful = true;
            serviceResponse.Message = "Order successfully retrieved!";

            return serviceResponse;
        }
 
        public async Task<ServiceResponse<GetOrderDto?>> UpdateOrder(Guid guid, ChangeOrderStatusDto updateOrderDto)
        {
            var order = await _context.Orders.Include(x => x.Items).FirstOrDefaultAsync(o => o.Id == guid);
            var serviceResponse = new ServiceResponse<GetOrderDto?>();

            if (order == null)
            {
                serviceResponse.Data = null;
                serviceResponse.StatusCode = 404;
                serviceResponse.Message = "Specified order was not found!";

                return serviceResponse;
            }

            if (!AdminCheck() && order.UserId != GetCurrentUserId())
            {
                serviceResponse.Data = null;
                serviceResponse.StatusCode = 401;
                serviceResponse.Message = "You do not have permission to update this order!";

                return serviceResponse;
            }

            if (!OrderStatusValidityCheck(updateOrderDto.OrderStatus, order))
            {
                serviceResponse.Data = null;
                serviceResponse.StatusCode = 400;
                serviceResponse.Message = "Cannot update order status to specified value!";

                return serviceResponse;
            }

            order.OrderStatus = updateOrderDto.OrderStatus;

            foreach (var item in order.Items)
            {
                Flower flower = await _context.Flowers.FirstOrDefaultAsync(f => f.Id == item.FlowerId);
                if (flower != null)
                    flower.NumberInStock += item.Amount;
            }

            await _context.SaveChangesAsync();
            var result = _mapper.Map<GetOrderDto>(order);
            User orderUser = await _context.Users.FirstOrDefaultAsync(u => u.Id == GetCurrentUserId());
            result.UserInfo = _mapper.Map<UserInfoDto>(orderUser);

            serviceResponse.Data = result;
            serviceResponse.StatusCode = 200;
            serviceResponse.IsSuccessful = true;
            serviceResponse.Message = "Order successfully updated!";

            return serviceResponse;
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
            bool isOperationValid = (orderStatus == OrderStatus.Cancelled && order.UserId == GetCurrentUserId()) || (orderStatus == OrderStatus.Declined && AdminCheck());
            return isOrderStatusSubmitted && isOperationValid;
        }
    }
}
