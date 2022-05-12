using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace PepsiPSK.Entities
{
    public class Flower
    {
        public Guid Id { get; set; }
        public string Name { get; set; }

        [Precision(6, 2)]
        [Range(0.01, 9999.99, ErrorMessage = "Value must be at least 0.01 and no more than 9999.99!")]
        public decimal Price { get; set; }

        [StringLength(500, ErrorMessage = "Description must not be longer than 500 characters!")]
        public string? Description { get; set; }

        public int Quantity { get; set; }

        public string? PhotoLink { get; set; }
        public ICollection<Transaction> Transactions { get; set; } = new List<Transaction>();
        public DateTime AdditionTime { get; set; } = DateTime.UtcNow;

        public User User { get; set; }
    }
}
