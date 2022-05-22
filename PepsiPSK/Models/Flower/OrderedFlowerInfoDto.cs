namespace PepsiPSK.Models.Flower
{
    public class OrderedFlowerInfoDto
    {
        public Guid FlowerId { get; set; }

        public string Name { get; set; }

        public decimal Price { get; set; }

        public string? PhotoLink { get; set; }

        public string? Description { get; set; }

        public int Amount { get; set; }

        public decimal Cost { get; set; }

        public OrderedFlowerInfoDto()
        {

        }
    }
}
