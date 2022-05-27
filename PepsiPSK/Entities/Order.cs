using PepsiPSK.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PepsiPSK.Entities
{
    public class Order
    {
        public Guid Id { get; set; }
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int OrderNumber { get; set; }

        [StringLength(500, ErrorMessage = "Description must not be longer than 500 characters!")]
        public string? Description { get; set; }

        public OrderStatus OrderStatus { get; set; } = OrderStatus.Submitted;

        public PaymentMethod PaymentMethod { get; set; }

        public decimal TotalCost { get; set; }

        public DateTime CreationTime { get; set; } = DateTime.UtcNow;

        public List<Flower> Flowers { get; set; }

        public virtual ICollection<FlowerItem>? Items { get; set; } = new List<FlowerItem>();

        [ConcurrencyCheck]
        public DateTime? LastModified { get; set; }
        public string UserId { get; set; }

        public DateTime? StatusModificationTime { get; set; }
    }
}
