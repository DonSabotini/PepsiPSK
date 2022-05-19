using PepsiPSK.Enums;

namespace PepsiPSK.Models.Order
{
    public class UpdateOrderDto
    {
        public Guid Id { get; set; }

        public OrderStatus OrderStatus { get; set; }
    }
}
