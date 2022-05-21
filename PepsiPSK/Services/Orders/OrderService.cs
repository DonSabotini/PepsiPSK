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
                var mappedOrdersForAdmin = new List<GetOrderDto>();

                foreach(var order in ordersForAdmin)
                {
                    var mappedOrderForAdmin = new GetOrderDto
                    {
                        Id = order.Id,
                        Description = order.Description,
                        OrderStatus = order.OrderStatus.ToString(),
                        TotalCost = order.TotalCost,
                        CreationTime = order.CreationTime,
                        UserId = order.UserId
                    };

                    mappedOrdersForAdmin.Add(mappedOrderForAdmin);
                }
     
                var orderIdsForAdmin = ordersForAdmin.Select(o => o.Id).ToList();
                var flowersForOrderForAdmin = await _context.FlowerOrders.Where(fo => orderIdsForAdmin.Contains(fo.OrderId)).ToListAsync();
                var flowerIdsForAdmin = flowersForOrderForAdmin.Select(f => f.FlowerId).ToList();
                var orderedFlowersForAdmin = await _context.Flowers.Where(flower => flowerIdsForAdmin.Contains(flower.Id)).ToListAsync();

                for (int i = 0; i < mappedOrdersForAdmin.Count; i++)
                {
                    List<FlowerOrder> filtered = flowersForOrderForAdmin.FindAll(fo => fo.OrderId == mappedOrdersForAdmin[i].Id);

                    for (int j = 0; j < filtered.Count; j++)
                    {
                        mappedOrdersForAdmin[i].OrderedFlowerInfo.Add(new OrderedFlowerInfoDto
                        {
                            FlowerId = orderedFlowersForAdmin[j].Id,
                            Name = orderedFlowersForAdmin[j].Name,
                            Price = orderedFlowersForAdmin[j].Price,
                            PhotoLink = orderedFlowersForAdmin[j].PhotoLink,
                            Description = orderedFlowersForAdmin[j].Description,
                            Amount = flowersForOrderForAdmin[j].Amount
                        });
                    }
                }

                foreach (var adminOrder in mappedOrdersForAdmin)
                {
                    var userForAdmin = await _context.Users.FirstOrDefaultAsync(u => u.Id == adminOrder.UserId);
                    adminOrder.UserInfo = _mapper.Map<UserInfoDto>(userForAdmin);
                }

                return mappedOrdersForAdmin;
            }

            var orders = await _context.Orders.Where(o => o.UserId == GetCurrentUserId()).Select(order => order).ToListAsync();
            var mappedOrders = new List<GetOrderDto>();

            foreach (var order in orders)
            {
                var mappedOrder = new GetOrderDto
                {
                    Id = order.Id,
                    Description = order.Description,
                    OrderStatus = order.OrderStatus.ToString(),
                    TotalCost = order.TotalCost,
                    CreationTime = order.CreationTime,
                    UserId = order.UserId
                };

                mappedOrders.Add(mappedOrder);
            }

            var orderIds = orders.Select(o => o.Id).ToList();
            var flowersForOrder = await _context.FlowerOrders.Where(fo => orderIds.Contains(fo.OrderId)).ToListAsync();
            var flowerIds = flowersForOrder.Select(f => f.FlowerId).ToList();
            var orderedFlowers = await _context.Flowers.Where(flower => flowerIds.Contains(flower.Id)).ToListAsync();

            for (int i = 0; i < mappedOrders.Count; i++)
            {
                List<FlowerOrder> filtered = flowersForOrder.FindAll(fo => fo.OrderId == mappedOrders[i].Id);

                for (int j = 0; j < filtered.Count; j++)
                {
                    mappedOrders[i].OrderedFlowerInfo.Add(new OrderedFlowerInfoDto
                    {
                        FlowerId = orderedFlowers[j].Id,
                        Name = orderedFlowers[j].Name,
                        Price = orderedFlowers[j].Price,
                        PhotoLink = orderedFlowers[j].PhotoLink,
                        Description = orderedFlowers[j].Description,
                        Amount = flowersForOrder[j].Amount
                    });
                }
            }

            foreach (var order in mappedOrders)
            {
                var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == order.UserId);
                order.UserInfo = _mapper.Map<UserInfoDto>(user);
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

            var mappedOrder = new GetOrderDto
            {
                Id = order.Id,
                Description = order.Description,
                OrderStatus = order.OrderStatus.ToString(),
                TotalCost = order.TotalCost,
                CreationTime = order.CreationTime,
                UserId = order.UserId
            };

            var flowersForOrder = await _context.FlowerOrders.Where(fo => fo.OrderId == mappedOrder.Id).ToListAsync();
            var flowerIds = flowersForOrder.Select(f => f.FlowerId).ToList();
            var orderedFlowers = await _context.Flowers.Where(flower => flowerIds.Contains(flower.Id)).ToListAsync();

            for (int i = 0; i < flowersForOrder.Count; i++)
            {
                mappedOrder.OrderedFlowerInfo.Add(new OrderedFlowerInfoDto
                {
                    FlowerId = orderedFlowers[i].Id,
                    Name = orderedFlowers[i].Name,
                    Price = orderedFlowers[i].Price,
                    PhotoLink = orderedFlowers[i].PhotoLink,
                    Description = orderedFlowers[i].Description,
                    Amount = flowersForOrder[i].Amount
                });
            }

            var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == mappedOrder.UserId);
            mappedOrder.UserInfo = _mapper.Map<UserInfoDto>(user);

            return mappedOrder;
        }

        public async Task<GetOrderDto> AddOrder(AddOrderDto addOrderDto)
        {
            Order newOrder = new();
            newOrder.Description = addOrderDto.Description;
            var sortedFlowersForOrder = addOrderDto.FlowersForOrder.OrderBy(f => f.FlowerId).ToList();
            var flowerIds = sortedFlowersForOrder.Select(f => f.FlowerId).ToList();
            var orderedFlowers = await _context.Flowers.Where(flower => flowerIds.Contains(flower.Id)).ToListAsync();

            decimal totalCost = 0;

            for (int i = 0; i < sortedFlowersForOrder.Count; i++)
            {
                orderedFlowers[i].NumberInStock -= sortedFlowersForOrder[i].Amount;
                totalCost += orderedFlowers[i].Price * sortedFlowersForOrder[i].Amount;
            }

            newOrder.Flowers = orderedFlowers;
            newOrder.TotalCost = totalCost;
            newOrder.UserId = GetCurrentUserId();
            await _context.Orders.AddAsync(newOrder);
            await _context.SaveChangesAsync();

            var addedOrder = await _context.Orders.FirstOrDefaultAsync(o => o.Id == newOrder.Id);
            var flowerOrders = await _context.FlowerOrders.Where(fo => fo.OrderId == addedOrder.Id).ToListAsync();

            var mappedOrder = new GetOrderDto
            {
                Id = addedOrder.Id,
                Description = addedOrder.Description,
                OrderStatus = addedOrder.OrderStatus.ToString(),
                TotalCost = addedOrder.TotalCost,
                CreationTime = addedOrder.CreationTime,
                UserId = addedOrder.UserId
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
                    Amount = sortedFlowersForOrder[i].Amount
                });
            }

            var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == mappedOrder.UserId);
            mappedOrder.UserInfo = _mapper.Map<UserInfoDto>(user);
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

            order.OrderStatus = updateOrderDto.OrderStatus;
            order.StatusModificationTime = DateTime.UtcNow;

            var flowerForOrder = await _context.FlowerOrders.Where(fo => fo.OrderId == order.Id).ToListAsync();
            var flowerIds = flowerForOrder.Select(f => f.FlowerId).ToList();
            var orderedFlowers = await _context.Flowers.Where(flower => flowerIds.Contains(flower.Id)).ToListAsync();

            var mappedOrder = new GetOrderDto
            {
                Id = order.Id,
                Description = order.Description,
                OrderStatus = order.OrderStatus.ToString(),
                TotalCost = order.TotalCost,
                CreationTime = order.CreationTime,
                UserId = order.UserId
            };

            for (int i = 0; i < flowerForOrder.Count; i++)
            {
                mappedOrder.OrderedFlowerInfo.Add(new OrderedFlowerInfoDto
                {
                    FlowerId = orderedFlowers[i].Id,
                    Name = orderedFlowers[i].Name,
                    Price = orderedFlowers[i].Price,
                    PhotoLink = orderedFlowers[i].PhotoLink,
                    Description = orderedFlowers[i].Description,
                    Amount = flowerForOrder[i].Amount
                });

                orderedFlowers[i].NumberInStock += flowerForOrder[i].Amount;
            }

            await _context.SaveChangesAsync();

            var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == mappedOrder.UserId);
            mappedOrder.UserInfo = _mapper.Map<UserInfoDto>(user);

            return mappedOrder;
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
