using AutoMapper;
using Microsoft.EntityFrameworkCore;
using PepsiPSK.Entities;
using PepsiPSK.Models.FlowerForOrder;
using PepsiPSK.Models.Order;
using PepsiPSK.Models.User;
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
                        OrderStatus = order.OrderStatus,
                        TotalCost = order.TotalCost,
                        CreationTime = order.CreationTime,
                        UserId = order.UserId
                    };

                    mappedOrdersForAdmin.Add(mappedOrderForAdmin);
                }
     
                var orderIdsForAdmin = ordersForAdmin.Select(o => o.Id).ToList();
                var flowersForOrderForAdmin = await _context.FlowerOrders.Where(fo => orderIdsForAdmin.Contains(fo.OrderId)).ToListAsync();

                for (int i = 0; i < mappedOrdersForAdmin.Count; i++)
                {
                    List<FlowerOrder> filtered = flowersForOrderForAdmin.FindAll(fo => fo.OrderId == mappedOrdersForAdmin[i].Id);

                    for (int j = 0; j < filtered.Count; j++)
                    {
                        mappedOrdersForAdmin[i].FlowerOrderInfo.Add(new FlowerForOrderDto
                        {
                            FlowerId = filtered[j].FlowerId,
                            Amount = filtered[j].Amount,
                        });
                    }
                }

                foreach (var adminOrder in mappedOrdersForAdmin)
                {
                    var userForAdmin = await _context.Users.FirstOrDefaultAsync(u => u.Id == adminOrder.UserId);
                    adminOrder.UserInfo = _mapper.Map<UserInfo>(userForAdmin);
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
                    OrderStatus = order.OrderStatus,
                    TotalCost = order.TotalCost,
                    CreationTime = order.CreationTime,
                    UserId = order.UserId
                };

                mappedOrders.Add(mappedOrder);
            }

            var orderIds = orders.Select(o => o.Id).ToList();
            var flowersForOrder = await _context.FlowerOrders.Where(fo => orderIds.Contains(fo.OrderId)).ToListAsync();

            for (int i = 0; i < mappedOrders.Count; i++)
            {
                List<FlowerOrder> filtered = flowersForOrder.FindAll(fo => fo.OrderId == mappedOrders[i].Id);

                for (int j = 0; j < filtered.Count; j++)
                {
                    mappedOrders[i].FlowerOrderInfo.Add(new FlowerForOrderDto
                    {
                        FlowerId = filtered[j].FlowerId,
                        Amount = filtered[j].Amount,
                    });
                }
            }

            foreach (var order in mappedOrders)
            {
                var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == order.UserId);
                order.UserInfo = _mapper.Map<UserInfo>(user);
            }

            return mappedOrders;
        }

        public async Task<GetOrderDto?> GetOrderById(Guid guid)
        {
            if (AdminCheck())
            {
                var orderForAdmin = await _context.Orders.FirstOrDefaultAsync(o => o.Id == guid);

                if (orderForAdmin == null)
                {
                    return null;
                }

                var mappedOrderForAdmin = new GetOrderDto
                {
                    Id = orderForAdmin.Id,
                    Description = orderForAdmin.Description,
                    OrderStatus = orderForAdmin.OrderStatus,
                    TotalCost = orderForAdmin.TotalCost,
                    CreationTime = orderForAdmin.CreationTime,
                    UserId = orderForAdmin.UserId
                };

                var flowerForOrderForAdmin = await _context.FlowerOrders.Where(fo => fo.OrderId == mappedOrderForAdmin.Id).ToListAsync();

                for (int i = 0; i < flowerForOrderForAdmin.Count; i++)
                {
                    mappedOrderForAdmin.FlowerOrderInfo.Add(new FlowerForOrderDto
                    {
                        FlowerId = flowerForOrderForAdmin[i].FlowerId,
                        Amount = flowerForOrderForAdmin[i].Amount
                    });
                }

                var userForAdmin = await _context.Users.FirstOrDefaultAsync(u => u.Id == mappedOrderForAdmin.UserId);
                mappedOrderForAdmin.UserInfo = _mapper.Map<UserInfo>(userForAdmin);

                return mappedOrderForAdmin;
            }

            var order = await _context.Orders.FirstOrDefaultAsync(o => o.Id == guid && o.UserId == GetCurrentUserId());

            if (order == null)
            {
                return null;
            }

            var mappedOrder = new GetOrderDto
            {
                Id = order.Id,
                Description = order.Description,
                OrderStatus = order.OrderStatus,
                TotalCost = order.TotalCost,
                CreationTime = order.CreationTime,
                UserId = order.UserId
            };

            var flowerForOrder = await _context.FlowerOrders.Where(fo => fo.OrderId == mappedOrder.Id).ToListAsync();

            for (int i = 0; i < flowerForOrder.Count; i++)
            {
                mappedOrder.FlowerOrderInfo.Add(new FlowerForOrderDto
                {
                    FlowerId = flowerForOrder[i].FlowerId,
                    Amount = flowerForOrder[i].Amount
                });
            }

            var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == mappedOrder.UserId);
            mappedOrder.UserInfo = _mapper.Map<UserInfo>(user);

            return mappedOrder;
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

            var mappedOrder = new GetOrderDto
            {
                Id = addedOrder.Id,
                Description = addedOrder.Description,
                OrderStatus = addedOrder.OrderStatus,
                TotalCost = addedOrder.TotalCost,
                CreationTime = addedOrder.CreationTime,
                UserId = addedOrder.UserId
            };

            for (int i = 0; i < flowerOrders.Count; i++)
            {
                flowerOrders[i].Amount = addOrderDto.Flowers[i].Amount;
                mappedOrder.FlowerOrderInfo.Add(new FlowerForOrderDto
                {
                    FlowerId = flowerOrders[i].FlowerId,
                    Amount = flowerOrders[i].Amount
                });
            }

            var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == mappedOrder.UserId);
            mappedOrder.UserInfo = _mapper.Map<UserInfo>(user);

            await _context.SaveChangesAsync();
            return mappedOrder;
        }

        public async Task<GetOrderDto?> UpdateOrder(UpdateOrderDto updateOrderDto)
        {
            if (AdminCheck())
            {
                var orderForAdmin = await _context.Orders.FirstOrDefaultAsync(o => o.Id == updateOrderDto.Id);

                if (orderForAdmin == null)
                {
                    return null;
                }

                orderForAdmin.Description = updateOrderDto.Description;
                orderForAdmin.OrderStatus = updateOrderDto.OrderStatus;

                var flowerForOrderForAdmin = await _context.FlowerOrders.Where(fo => fo.OrderId == orderForAdmin.Id).ToListAsync();

                var mappedOrderForAdmin = new GetOrderDto
                {
                    Id = orderForAdmin.Id,
                    Description = orderForAdmin.Description,
                    OrderStatus = orderForAdmin.OrderStatus,
                    TotalCost = orderForAdmin.TotalCost,
                    CreationTime = orderForAdmin.CreationTime,
                    UserId = orderForAdmin.UserId
                };

                for (int i = 0; i < flowerForOrderForAdmin.Count; i++)
                {
                    mappedOrderForAdmin.FlowerOrderInfo.Add(new FlowerForOrderDto
                    {
                        FlowerId = flowerForOrderForAdmin[i].FlowerId,
                        Amount = flowerForOrderForAdmin[i].Amount
                    });
                }

                await _context.SaveChangesAsync();

                var userForAdmin = await _context.Users.FirstOrDefaultAsync(u => u.Id == mappedOrderForAdmin.UserId);
                mappedOrderForAdmin.UserInfo = _mapper.Map<UserInfo>(userForAdmin);
                return mappedOrderForAdmin;
            }

            var order = await _context.Orders.FirstOrDefaultAsync(o => o.Id == updateOrderDto.Id && o.UserId == GetCurrentUserId());

            if (order == null)
            {
                return null;
            }

            order.Description = updateOrderDto.Description;
            order.OrderStatus = updateOrderDto.OrderStatus;

            var flowerForOrder = await _context.FlowerOrders.Where(fo => fo.OrderId == order.Id).ToListAsync();

            var mappedOrder = new GetOrderDto
            {
                Id = order.Id,
                Description = order.Description,
                OrderStatus = order.OrderStatus,
                TotalCost = order.TotalCost,
                CreationTime = order.CreationTime,
                UserId = order.UserId
            };

            for (int i = 0; i < flowerForOrder.Count; i++)
            {
                mappedOrder.FlowerOrderInfo.Add(new FlowerForOrderDto
                {
                    FlowerId = flowerForOrder[i].FlowerId,
                    Amount = flowerForOrder[i].Amount
                });
            }

            await _context.SaveChangesAsync();

            var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == mappedOrder.UserId);
            mappedOrder.UserInfo = _mapper.Map<UserInfo>(user);
            return mappedOrder;
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
