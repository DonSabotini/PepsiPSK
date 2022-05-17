using PepsiPSK.Enums;
using PepsiPSK.Models.FlowerForOrder;
using PepsiPSK.Models.User;

namespace PepsiPSK.Models.Order
{
    public class GetOrderDto
    {
        public Guid Id { get; set; }

        public string? Description { get; set; }

        public OrderStatus OrderStatus { get; set; } = OrderStatus.Pending;

        public decimal TotalCost { get; set; }

        public DateTime CreationTime { get; set; } = DateTime.UtcNow;

        public List<FlowerForOrderDto> FlowerOrderInfo { get; set; } = new List<FlowerForOrderDto>();

        public string UserId { get; set; }

        public UserInfo UserInfo { get; set; }

        public GetOrderDto()
        {

        }
    }
}
