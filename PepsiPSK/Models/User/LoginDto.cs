using System.ComponentModel.DataAnnotations;

namespace PepsiPSK.Models.User
{
    public class LoginDto
    {
        [EmailAddress]
        [Required(ErrorMessage = "Email is required!")]
        public string LoginEmail { get; set; }

        [Required(ErrorMessage = "Password is required!")]
        public string LoginPassword { get; set; }
    }
}
