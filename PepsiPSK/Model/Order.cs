namespace PepsiPSK.Model
{
    public class Order
    {
        public Guid Id { get; set; }

        public string Description { get; set; }
        public List<Transaction> Transactions { get; set; } 

    }
}
