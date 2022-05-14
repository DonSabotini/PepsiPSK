namespace PepsiPSK.Models.Transaction
{
    public class UpdateTransactionDto
    {
        public Guid Id { get; set; }
        public Guid FlowerId { get; set; }
        public int Diference { get; set; }
    }
}
