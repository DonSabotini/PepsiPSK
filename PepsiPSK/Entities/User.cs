using Microsoft.AspNetCore.Identity;

namespace PepsiPSK.Entities
{
    public class User : IdentityUser
    {
        public string FirstName { get; set; }

        public string LastName { get; set; }

        public ICollection<Order> SubmittedOrders { get; set; } = new List<Order>();

        public ICollection<Flower> FlowersForSelling { get; set; } = new List<Flower>();

        public ICollection<Transaction> PersonalTransactions { get; set; } = new List<Transaction>();
    }
}
