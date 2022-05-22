using AutoMapper;
using Microsoft.EntityFrameworkCore;
using PepsiPSK.Entities;
using PepsiPSK.Models.Order;
using PepsiPSK.Models.User;
using PepsiPSK.Utils.Authentication;
using Pepsi.Data;
using System.Security;
using PepsiPSK.Enums;
using PepsiPSK.Models.Flower;

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
            if (AdminCheck())
            {
                var ordersForAdmin = await _context.Orders.Select(order => order).ToListAsync();
                var orderIdsForAdmin = ordersForAdmin.Select(o => o.Id).ToList();
                var mappedOrdersForAdmin = new List<GetOrderDto>();

                foreach(var order in ordersForAdmin)
                {
                    var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == order.UserId);

                    var mappedOrderForAdmin = new GetOrderDto
                    {
                        Id = order.Id,
                        Description = order.Description,
                        OrderStatus = order.OrderStatus.ToString(),
                        PaymentMethod = order.PaymentMethod.ToString(),
                        TotalCost = order.TotalCost,
                        CreationTime = order.CreationTime,
                        UserId = order.UserId,
                        UserInfo = _mapper.Map<UserInfoDto>(user)
                    };

                    mappedOrdersForAdmin.Add(mappedOrderForAdmin);
                }

                var orderedFlowersForAdmin = await _context.Flowers.Include(fo => fo.FlowerOrders)
                    .Where(flower => flower.FlowerOrders.Any(flowerOrder => orderIdsForAdmin.Contains(flowerOrder.OrderId))).ToListAsync();

                for (int i = 0; i < mappedOrdersForAdmin.Count; i++)
                {
                    for (int j = 0; j < orderedFlowersForAdmin.Count; j++)
                    {
                        var flowerAmount = orderedFlowersForAdmin[j].FlowerOrders.FirstOrDefault(fo => fo.FlowerId == orderedFlowersForAdmin[j].Id).Amount;

                        mappedOrdersForAdmin[i].OrderedFlowerInfo.Add(new OrderedFlowerInfoDto
                        {
                            FlowerId = orderedFlowersForAdmin[j].Id,
                            Name = orderedFlowersForAdmin[j].Name,
                            Price = orderedFlowersForAdmin[j].Price,
                            PhotoLink = orderedFlowersForAdmin[j].PhotoLink,
                            Description = orderedFlowersForAdmin[j].Description,
                            Amount = flowerAmount,
                            Cost = orderedFlowersForAdmin[j].Price * flowerAmount
                        });
                    }
                }

                return mappedOrdersForAdmin;
            }

            var orders = await _context.Orders.Where(o => o.UserId == GetCurrentUserId()).Select(order => order).ToListAsync();
            var orderIds = orders.Select(o => o.Id).ToList();
            var mappedOrders = new List<GetOrderDto>();

            foreach (var order in orders)
            {
                var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == order.UserId);

                var mappedOrder = new GetOrderDto
                {
                    Id = order.Id,
                    Description = order.Description,
                    OrderStatus = order.OrderStatus.ToString(),
                    PaymentMethod = order.PaymentMethod.ToString(),
                    TotalCost = order.TotalCost,
                    CreationTime = order.CreationTime,
                    UserId = order.UserId,
                    UserInfo = _mapper.Map<UserInfoDto>(user)
                };

                mappedOrders.Add(mappedOrder);
            }

            var orderedFlowers = await _context.Flowers.Include(fo => fo.FlowerOrders)
                .Where(flower => flower.FlowerOrders.Any(flowerOrder => orderIds.Contains(flowerOrder.OrderId))).ToListAsync();

            for (int i = 0; i < mappedOrders.Count; i++)
            {
                for (int j = 0; j < orderedFlowers.Count; j++)
                {
                    var flowerAmount = orderedFlowers[j].FlowerOrders.FirstOrDefault(fo => fo.FlowerId == orderedFlowers[j].Id).Amount;

                    mappedOrders[i].OrderedFlowerInfo.Add(new OrderedFlowerInfoDto
                    {
                        FlowerId = orderedFlowers[j].Id,
                        Name = orderedFlowers[j].Name,
                        Price = orderedFlowers[j].Price,
                        PhotoLink = orderedFlowers[j].PhotoLink,
                        Description = orderedFlowers[j].Description,
                        Amount = flowerAmount,
                        Cost = orderedFlowers[j].Price * flowerAmount
                    });
                }
            }

            return mappedOrders;
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

            var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == order.UserId);

            var mappedOrder = new GetOrderDto
            {
                Id = order.Id,
                Description = order.Description,
                OrderStatus = order.OrderStatus.ToString(),
                PaymentMethod = order.PaymentMethod.ToString(),
                TotalCost = order.TotalCost,
                CreationTime = order.CreationTime,
                UserId = order.UserId,
                UserInfo = _mapper.Map<UserInfoDto>(user)
            };

            var orderedFlowers = await _context.Flowers.Include(fo => fo.FlowerOrders)
                .Where(flower => flower.FlowerOrders.Any(flowerOrder => flowerOrder.OrderId == order.Id)).ToListAsync();

            for (int i = 0; i < orderedFlowers.Count; i++)
            {
                var flowerAmount = orderedFlowers[i].FlowerOrders.FirstOrDefault(fo => fo.FlowerId == orderedFlowers[i].Id).Amount;

                mappedOrder.OrderedFlowerInfo.Add(new OrderedFlowerInfoDto
                {
                    FlowerId = orderedFlowers[i].Id,
                    Name = orderedFlowers[i].Name,
                    Price = orderedFlowers[i].Price,
                    PhotoLink = orderedFlowers[i].PhotoLink,
                    Description = orderedFlowers[i].Description,
                    Amount = flowerAmount,
                    Cost = orderedFlowers[i].Price * flowerAmount
                });
            }

            return mappedOrder;
        }

        public async Task<GetOrderDto> AddOrder(AddOrderDto addOrderDto)
        {
            var sortedFlowersForOrder = addOrderDto.FlowersForOrder.OrderBy(f => f.FlowerId).ToList();
            var flowerIds = sortedFlowersForOrder.Select(f => f.FlowerId).ToList();
            var orderedFlowers = await _context.Flowers.Where(flower => flowerIds.Contains(flower.Id)).ToListAsync();

            for (int i = 0; i < sortedFlowersForOrder.Count; i++)
            {
                orderedFlowers[i].NumberInStock -= sortedFlowersForOrder[i].Amount;
            }

            Order newOrder = new();
            newOrder.Flowers = orderedFlowers;
            newOrder.Description = addOrderDto.Description;
            newOrder.TotalCost = addOrderDto.TotalCost;
            newOrder.PaymentMethod = addOrderDto.PaymentMethod;
            newOrder.UserId = GetCurrentUserId();
            await _context.Orders.AddAsync(newOrder);
            await _context.SaveChangesAsync();

            var addedOrder = await _context.Orders.FirstOrDefaultAsync(o => o.Id == newOrder.Id);
            var flowerOrders = await _context.FlowerOrders.Where(fo => fo.OrderId == addedOrder.Id).ToListAsync();

            var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == addedOrder.UserId);

            var mappedOrder = new GetOrderDto
            {
                Id = addedOrder.Id,
                Description = addedOrder.Description,
                OrderStatus = addedOrder.OrderStatus.ToString(),
                TotalCost = addedOrder.TotalCost,
                PaymentMethod = addOrderDto.PaymentMethod.ToString(),
                CreationTime = addedOrder.CreationTime,
                UserId = addedOrder.UserId,
                UserInfo = _mapper.Map<UserInfoDto>(user)
            };

            for (int i = 0; i < sortedFlowersForOrder.Count; i++)
            {
                flowerOrders[i].Amount = sortedFlowersForOrder[i].Amount;

                mappedOrder.OrderedFlowerInfo.Add(new OrderedFlowerInfoDto
                {
                    FlowerId = orderedFlowers[i].Id,
                    Name = orderedFlowers[i].Name,
                    Price = orderedFlowers[i].Price,
                    PhotoLink = orderedFlowers[i].PhotoLink,
                    Description = orderedFlowers[i].Description,
                    Amount = sortedFlowersForOrder[i].Amount,
                    Cost = orderedFlowers[i].Price * sortedFlowersForOrder[i].Amount
                });
            }

            await _context.SaveChangesAsync();

            return mappedOrder;
        }

        public async Task<GetOrderDto?> UpdateOrder(Guid guid, ChangeOrderStatusDto updateOrderDto)
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

            if (!OrderStatusValidityCheck(updateOrderDto.OrderStatus, order))
            {
                throw new SecurityException();
            }

            try
            {
                order.OrderStatus = updateOrderDto.OrderStatus;
                order.StatusModificationTime = DateTime.UtcNow;

                var orderedFlowers = await _context.Flowers.Include(fo => fo.FlowerOrders)
                    .Where(flower => flower.FlowerOrders.Any(flowerOrder => flowerOrder.OrderId == order.Id)).ToListAsync();

                var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == order.UserId);

                var mappedOrder = new GetOrderDto
                {
                    Id = order.Id,
                    Description = order.Description,
                    OrderStatus = order.OrderStatus.ToString(),
                    TotalCost = order.TotalCost,
                    CreationTime = order.CreationTime,
                    UserId = order.UserId,
                    UserInfo = _mapper.Map<UserInfoDto>(user)
                };

                for (int i = 0; i < orderedFlowers.Count; i++)
                {
                    var flowerAmount = orderedFlowers[i].FlowerOrders.FirstOrDefault(fo => fo.FlowerId == orderedFlowers[i].Id).Amount;

                    mappedOrder.OrderedFlowerInfo.Add(new OrderedFlowerInfoDto
                    {
                        FlowerId = orderedFlowers[i].Id,
                        Name = orderedFlowers[i].Name,
                        Price = orderedFlowers[i].Price,
                        PhotoLink = orderedFlowers[i].PhotoLink,
                        Description = orderedFlowers[i].Description,
                        Amount = flowerAmount,
                        Cost = orderedFlowers[i].Price * flowerAmount
                    });

                    orderedFlowers[i].NumberInStock += flowerAmount;
                }

                await _context.SaveChangesAsync();

                return mappedOrder;
            }
            catch (DbUpdateConcurrencyException ex)
            {
                throw ex;
            }
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
