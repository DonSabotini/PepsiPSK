using Microsoft.AspNetCore.Identity;

namespace PepsiPSK.Entities
{
    public class User : IdentityUser
    {
        public string FirstName { get; set; }

        public string LastName { get; set; }

        public ICollection<Order> Orders { get; set; } = new List<Order>();

        public ICollection<Flower> Flowers{ get; set; } = new List<Flower>();
    }
}
