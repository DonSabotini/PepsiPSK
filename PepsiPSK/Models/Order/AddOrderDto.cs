using PepsiPSK.Enums;
using PepsiPSK.Models.FlowerForOrder;
using System.ComponentModel.DataAnnotations;

namespace PepsiPSK.Models.Order
{
    public class AddOrderDto
    {
        [StringLength(500, ErrorMessage = "Description must not be longer than 500 characters!")]
        public string? Description { get; set; }

        public List<AddFlowerForOrderDto> FlowersForOrder { get; set; }

        public PaymentMethod PaymentMethod { get; set; }
    }
}
