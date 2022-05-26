using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PepsiPSK.Entities
{
    public class Flower
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        [Precision(6, 2)]
        [Range(0.01, double.MaxValue, ErrorMessage = "Value must be at least 0.01!")]
        public decimal Price { get; set; }

        [StringLength(500, ErrorMessage = "Description must not be longer than 500 characters!")]
        public string? Description { get; set; }

        [Range(0, int.MaxValue, ErrorMessage = "Number in stock cannot be negative!")]
        public int NumberInStock { get; set; }

        public Guid? PhotoId { get; set; }

        public DateTime AdditionTime { get; set; } = DateTime.UtcNow;

        public List<Order> Orders { get; set; }

        public DateTime? LastModified { get; set; }

        public List<FlowerOrder> FlowerOrders { get; set; }
    }
}
