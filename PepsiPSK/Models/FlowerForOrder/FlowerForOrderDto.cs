using System.ComponentModel.DataAnnotations;

namespace PepsiPSK.Models.FlowerForOrder
{
    public class FlowerForOrderDto
    {
        public Guid FlowerId { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "Amount must be at least one!")]
        public int Amount { get; set; }
    }
}
