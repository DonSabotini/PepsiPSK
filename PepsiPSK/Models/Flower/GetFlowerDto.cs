namespace PepsiPSK.Models.Flower
{
    public class GetFlowerDto
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public decimal Price { get; set; }

        public string? Description { get; set; }

        public int NumberInStock { get; set; }

        public string? PhotoLink { get; set; }

        public DateTime AdditionTime { get; set; }

        public DateTime? LastModified { get; set; }
    }
}
