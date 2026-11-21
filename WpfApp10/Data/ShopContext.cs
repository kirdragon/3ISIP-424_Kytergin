
using System.Data.Entity;
using WpfApp10.Models;

namespace WpfApp10.Data
{
    public class ShopContext : DbContext
    {
        static ShopContext()
        {
            Database.SetInitializer<ShopContext>(null);
        }

        public ShopContext() : base("ShopConnection")
        {
        }

        public DbSet<Product> Products { get; set; }
        public DbSet<CartItem> CartItems { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }

        protected override void OnModelCreating(
            DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Product>()
                .Property(p => p.Price).HasPrecision(10, 2);

            modelBuilder.Entity<Order>()
                .Property(o => o.TotalPrice).HasPrecision(10, 2);

            modelBuilder.Entity<OrderItem>()
                .Property(o => o.Price).HasPrecision(10, 2);

            modelBuilder.Entity<CartItem>()
                .HasRequired(c => c.Product)
                .WithMany()
                .HasForeignKey(c => c.ProductId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<OrderItem>()
                .HasRequired(i => i.Order)
                .WithMany(o => o.OrderItems)
                .HasForeignKey(i => i.OrderId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<OrderItem>()
                .HasRequired(i => i.Product)
                .WithMany()
                .HasForeignKey(i => i.ProductId)
                .WillCascadeOnDelete(false);
        }
    }
}
