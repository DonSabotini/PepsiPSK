using System.ComponentModel.DataAnnotations;

namespace PepsiPSK.Entities
{
    public class Flower
    {
        public Guid Id { get; set; }
        public string Name { get; set; }

        [Range(0.01, double.MaxValue)]
        public double Price { get; set; }

        [StringLength(500, ErrorMessage = "Description must not be longer than {0} characters!")]
        public string? Description { get; set; }

        [Range(0, int.MaxValue, ErrorMessage = "Value must be an integer greater or equal to zero!")]
        public int Quantity { get; set; }
        public string? PhotoLink { get; set; }
        public ICollection<Transaction> Transactions { get; set; } = new List<Transaction>();
        public DateTime AdditionTime { get; set; } = DateTime.UtcNow;
    }
}
