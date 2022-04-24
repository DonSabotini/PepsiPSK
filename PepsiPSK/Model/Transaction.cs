namespace PepsiPSK.Model
{
    public class Transaction
    {
        public Guid Id { get; set; }
        public Guid FlowerId { get; set; }
        public int Diference { get; set; }
    }
}
