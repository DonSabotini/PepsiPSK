using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace PepsiPSK.Entities
{
    public class User : IdentityUser
    {
        public string FirstName { get; set; }

        public string LastName { get; set; }

        public List<Order> Orders { get; set; } = new List<Order>();

        [Timestamp]
        public byte[] Version { get; set; }
    }
}
