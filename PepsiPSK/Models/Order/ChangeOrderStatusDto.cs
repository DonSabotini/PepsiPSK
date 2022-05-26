using PepsiPSK.Enums;

namespace PepsiPSK.Models.Order
{
    public class ChangeOrderStatusDto
    {
        public DateTime? LastModified { get; set; }
        public OrderStatus OrderStatus { get; set; }
    }
}
