using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace PepsiPSK.Entities
{
    public class User : IdentityUser
    {
        [Required(ErrorMessage = "First name is required!")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Last name is required!")]
        public string LastName { get; set; }

        public List<Order> Orders { get; set; } = new List<Order>();
    }
}
