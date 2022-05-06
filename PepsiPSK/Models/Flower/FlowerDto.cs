namespace PepsiPSK.Models.Flower
{
    public class FlowerDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public double Price { get; set; }
        public string? Description { get; set; }
        public int Quantity { get; set; }
        public string? PhotoLink { get; set; }
    }
}