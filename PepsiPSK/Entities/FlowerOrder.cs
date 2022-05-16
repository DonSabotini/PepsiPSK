using System.ComponentModel.DataAnnotations;

namespace PepsiPSK.Entities
{
    public class FlowerOrder
    {
        public Guid FlowerId { get; set; }

        public Guid OrderId { get; set; }

        public int Amount { get; set; }
    }
}
