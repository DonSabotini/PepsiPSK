namespace PepsiPSK.Models.Flower
{
    public class OrderedFlowerInfoDto
    {
        public string Name { get; set; }

        public decimal Price { get; set; }

        public string? Description { get; set; }

        public int Amount { get; set; }

        public OrderedFlowerInfoDto()
        {

        }
    }
}
