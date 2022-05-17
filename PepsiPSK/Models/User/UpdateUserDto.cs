namespace PepsiPSK.Models.User
{
    public class UpdateUserDto
    {
        public string Id { get; set; }

        public string UserName { get; set; }

        public string Password { get; set; }

        public string PasswordRepeated { get; set; }
    }
}
