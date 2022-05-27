using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace PepsiPSK.Entities
{
    public class User : IdentityUser
    {
        public string FirstName { get; set; }

        public string LastName { get; set; }

        public virtual ICollection<Order> Orders { get; set; }

        [ConcurrencyCheck]
        public DateTime? LastModified { get; set; }
    }
}
