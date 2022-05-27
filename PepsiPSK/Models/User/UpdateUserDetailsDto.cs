namespace PepsiPSK.Models.User
{
    public class UpdateUserDetailsDto
    {
        public string? NewUsername { get; set; }

        public string? NewFirstName { get; set; }

        public string? NewLastName { get; set; }

        public DateTime? LastModified { get; set; }
    }
}
