using Microsoft.EntityFrameworkCore;
using PepsiPSK.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PSIShoppingEngine.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options) { }
        public DbSet<Photo> Photos { get; set; }
        public DbSet<Flower> Flowers { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<Transaction> Transactions { get; set; }

    }
}
