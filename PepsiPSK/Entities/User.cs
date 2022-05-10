using Microsoft.AspNetCore.Identity;

namespace PepsiPSK.Entities
{
    public class User : IdentityUser
    {
        public string FirstName { get; set; }

        public string LastName { get; set; }

        public List<Order> SubmittedOrders { get; set; } = new List<Order>();

        public List<Flower> FlowersForSelling { get; set; } = new List<Flower>();
    }
}
