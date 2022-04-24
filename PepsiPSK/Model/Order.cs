namespace PepsiPSK.Model
{
    public class Order
    {
        public Guid Id { get; set; }
        public string? Description { get; set; }
        public ICollection<Transaction> Transactions { get; set; } 

    }
}
