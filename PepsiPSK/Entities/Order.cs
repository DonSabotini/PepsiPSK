using Microsoft.EntityFrameworkCore;
using PepsiPSK.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PepsiPSK.Entities
{
    public class Order
    {
        public Guid Id { get; set; }

        [StringLength(500, ErrorMessage = "Description must not be longer than 500 characters!")]
        public string? Description { get; set; }

        public ICollection<Transaction> Transactions { get; set; }
        public OrderStatus OrderStatus { get; set; } = OrderStatus.Pending;

        public decimal TotalCost { get; set; }

        public DateTime CreationTime { get; set; } = DateTime.UtcNow;

        public User? User { get; set; }
    }
}
