namespace PepsiPSK.Models.User
{
    public class ChangePasswordDto
    {
        public string OldPassword { get; set; }

        public string NewPassword { get; set; }

        public string NewPasswordRepeated { get; set; }

        public DateTime? LastModified { get; set; }
    }
}
