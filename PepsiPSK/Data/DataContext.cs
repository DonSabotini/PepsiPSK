using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using PepsiPSK.Entities;

namespace Pepsi.Data
{
    public class DataContext : IdentityDbContext<User>
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options) { }
        public DbSet<Photo> Photos { get; set; }
        public DbSet<Flower> Flowers { get; set; }

        public DbSet<Order> Orders { get; set; }

        public DbSet<FlowerOrder> FlowerOrders { get; set; }
        
        public DbSet<ActionRecord> ActionRecords { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Flower>()
           .HasMany(f => f.Orders)
           .WithMany(f => f.Flowers)
           .UsingEntity<FlowerOrder>(
               flowerOrder =>
               {
                   flowerOrder.Property(fo => fo.Amount);
                   flowerOrder.HasKey(fo => new { fo.FlowerId, fo.OrderId });
               });

            modelBuilder.Entity<Flower>()
                   .UseXminAsConcurrencyToken();

            modelBuilder.Entity<Order>()
               .UseXminAsConcurrencyToken();

            modelBuilder.Entity<User>()
               .UseXminAsConcurrencyToken();

            base.OnModelCreating(modelBuilder);
        }
    }
}
