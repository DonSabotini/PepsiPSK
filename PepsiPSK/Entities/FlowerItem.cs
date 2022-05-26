using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PepsiPSK.Entities
{
    public class FlowerItem
    {
        public Guid Id { get; set; }

        public Guid FlowerId { get; set; }
        public string Name { get; set; }

        [Precision(6, 2)]
        [Range(0.01, double.MaxValue, ErrorMessage = "Value must be at least 0.01!")]
        public decimal Price { get; set; }

        [StringLength(500, ErrorMessage = "Description must not be longer than 500 characters!")]
        public string? Description { get; set; }
        
        [Range(1, int.MaxValue, ErrorMessage = "Amount must be at least one!")]
        public int Amount { get; set; }
        public Guid OrderId { get; set; }
    }
}
