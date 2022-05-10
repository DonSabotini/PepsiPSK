using PepsiPSK.Enums;
using System.ComponentModel.DataAnnotations;

namespace PepsiPSK.Entities
{
    public class Order
    {
        public Guid Id { get; set; }

        [StringLength(500, ErrorMessage = "Description must not be longer than {0} characters!")]
        public string? Description { get; set; }
        public ICollection<Transaction> Transactions { get; set; }
        public OrderStatus OrderStatus { get; set; } = OrderStatus.Pending;

        public double TotalCost { get; set; }
        public DateTime CreationTime { get; set; } = DateTime.UtcNow;
    }
}
