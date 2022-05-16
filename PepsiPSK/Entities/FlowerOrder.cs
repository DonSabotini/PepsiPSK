using System.ComponentModel.DataAnnotations;

namespace PepsiPSK.Entities
{
    public class FlowerOrder
    {
        public Guid FlowerId { get; set; }

        public Guid OrderId { get; set; }

        [Range(0, int.MaxValue, ErrorMessage = "Amount cannot be negative!")]
        public int Amount { get; set; }
    }
}
