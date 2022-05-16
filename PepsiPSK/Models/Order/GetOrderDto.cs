using PepsiPSK.Enums;

namespace PepsiPSK.Models.Order
{
    public class GetOrderDto
    {
        public Guid Id { get; set; }

        public string? Description { get; set; }

        public OrderStatus OrderStatus { get; set; } = OrderStatus.Pending;

        public decimal TotalCost { get; set; }

        public DateTime CreationTime { get; set; } = DateTime.UtcNow;

        public string UserId { get; set; }
    }
}
