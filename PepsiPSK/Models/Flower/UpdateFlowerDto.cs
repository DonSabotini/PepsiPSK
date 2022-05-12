using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace PepsiPSK.Models.Flower
{
    public class UpdateFlowerDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }

        [Precision(6, 2)]
        [Range(0.01, 10000.00, ErrorMessage = "Value must be at least 0.01 and no more than 10000!")]
        public decimal Price { get; set; }

        [StringLength(500, ErrorMessage = "Description must not be longer than 500 characters!")]
        public string? Description { get; set; }

        [Range(0, 1000, ErrorMessage = "Value must be an integer greater or equal to 0 and not greater than 1000!")]
        public int Quantity { get; set; }

        public string? PhotoLink { get; set; }
    }
}