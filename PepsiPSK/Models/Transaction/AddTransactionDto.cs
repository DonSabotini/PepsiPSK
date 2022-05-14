namespace PepsiPSK.Models.Transaction
{
    public class AddTransactionDto
    {
        public Guid FlowerId { get; set; }
        public int Diference { get; set; }
    }
}
