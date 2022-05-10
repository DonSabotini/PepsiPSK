using System.ComponentModel.DataAnnotations;

namespace PepsiPSK.Models.User
{
    public class RegistrationDto
    {
        [Required(ErrorMessage = "Username is required!")]
        public string RegistrationUsername { get; set; }

        [EmailAddress]
        [Required(ErrorMessage = "Email is required!")]
        public string RegistrationEmail { get; set; }

        [Required(ErrorMessage = "Password is required!")]
        public string RegistrationPassword { get; set; }

        [Required(ErrorMessage = "First name is required!")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Last name is required!")]
        public string LastName { get; set; }
    }
}
