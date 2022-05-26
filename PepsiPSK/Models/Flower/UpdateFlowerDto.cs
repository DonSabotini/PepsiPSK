using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace PepsiPSK.Models.Flower
{
    public class UpdateFlowerDto
    {
        public string Name { get; set; }

        [Precision(6, 2)]
        [Range(0.01, 9999.99, ErrorMessage = "Value must be at least 0.01 and no more than 10000.00!")]
        public decimal Price { get; set; }

        [StringLength(500, ErrorMessage = "Description must not be longer than 500 characters!")]
        public string? Description { get; set; }

        public Guid? PhotoId { get; set; }

        [Range(0, int.MaxValue, ErrorMessage = "Number in stock cannot be negative!")]
        public int NumberInStock { get; set; }

        public DateTime? LastModified { get; set; }
    }
}