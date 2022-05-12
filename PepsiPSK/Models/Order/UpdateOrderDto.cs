using System.ComponentModel.DataAnnotations;

namespace PepsiPSK.Models.Order
{
    public class UpdateOrderDto
    {
        public Guid Id { get; set; }

        [StringLength(500, ErrorMessage = "Description must not be longer than 500 characters!")]
        public string? Description { get; set; }
    }
}
