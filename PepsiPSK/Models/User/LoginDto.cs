using System.ComponentModel.DataAnnotations;

namespace PepsiPSK.Models.User
{
    public class LoginDto
    {
        [Required(ErrorMessage = "Username is required!")]
        public string LoginUsername { get; set; }

        [Required(ErrorMessage = "Password is required!")]
        public string LoginPassword { get; set; }
    }
}
