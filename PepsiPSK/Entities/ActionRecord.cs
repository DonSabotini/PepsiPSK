namespace PepsiPSK.Entities
{
    public class ActionRecord
    {
        public Guid Id { get; set; }

        public string? UserName { get; set; }
        
        public string? UserId { get; set; }
        
        public string? Role { get; set; }

        public DateTime Time { get; set; }

        public string? UsedMethod { get; set; }
    }
}
