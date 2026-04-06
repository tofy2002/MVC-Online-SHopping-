using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace OnlineShopping.Data
{
    public class ShoppingContext : IdentityDbContext<Customer>
    {
        public ShoppingContext(DbContextOptions<ShoppingContext> options)
            : base(options)
        {
        }
        //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) 
        //{ // base.OnConfiguring(optionsBuilder); //}

        public DbSet<Product> Products { get; set; }
        public DbSet<Address> Addresses { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }
        public DbSet<Category> Categories { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Category>().HasQueryFilter(c => !c.IsDeleted);
            modelBuilder.Entity<Product>().HasQueryFilter(c => !c.IsDeleted);
            modelBuilder.Entity<Address>().HasQueryFilter(c => !c.IsDeleted);
            modelBuilder.Entity<Address>().HasOne(c => c.Customer).WithMany(c => c.Addresses).HasForeignKey(c => c.UserId).OnDelete(DeleteBehavior.Restrict); 
            modelBuilder.Entity<Order>().HasOne(c => c.Customer).WithMany(c=>c.Orders).HasForeignKey(c=>c.UserId).OnDelete(DeleteBehavior.Restrict); 
            modelBuilder.Entity<Order>().HasOne(c => c.ShippingAddress).WithMany(c=>c.Orders).HasForeignKey(c=>c.ShippingAddressId).OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<OrderItem>().HasOne(c => c.Order).WithMany(c => c.OrderItems).HasForeignKey(c => c.OrderId); 
            modelBuilder.Entity<OrderItem>().HasOne(c=>c.Product).WithMany(c=>c.OrderItems).HasForeignKey(c=>c.ProductId).OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<Product>().HasOne(c=>c.Category).WithMany(c=>c.Products).HasForeignKey(c=>c.CategoryId).OnDelete(DeleteBehavior.Restrict); 
            modelBuilder.Entity<Category>().HasOne(c=>c.ParentCategory).WithMany(c=>c.SubCategories).HasForeignKey(c=>c.ParentCategoryId).OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<Customer>().HasIndex(c => c.Email).IsUnique();
            modelBuilder.Entity<Order>().HasIndex(c => c.OrderNo).IsUnique();
            modelBuilder.Entity<Order>().Property(c => c.Status).IsRequired();
            modelBuilder.Entity<Product>().HasIndex(c => c.SKU).IsUnique();
            //modelBuilder.Entity<Category>().HasIndex(c => c.Name).IsUnique();
            modelBuilder.Entity<Address>().Property(c => c.City).IsRequired();
            modelBuilder.Entity<Address>().Property(c => c.UserId).IsRequired();
            modelBuilder.Entity<Address>().Property(c => c.Country).IsRequired();
            modelBuilder.Entity<Address>().Property(c => c.Street).IsRequired();
            modelBuilder.Entity<Product>().Property(c => c.SKU).IsRequired();
            modelBuilder.Entity<Product>().Property(c => c.Name).IsRequired();
            modelBuilder.Entity<Customer>().Property(c => c.FullName).IsRequired();
            modelBuilder.Entity<OrderItem>().Property(c => c.UnitPrice).IsRequired();
            modelBuilder.Entity<OrderItem>().Property(c => c.Quantity).IsRequired();
            modelBuilder.Entity<OrderItem>().Property(c => c.LineTotal).IsRequired();
            modelBuilder.Entity<Order>().Property(c => c.ShippingAddressId).IsRequired();
            modelBuilder.Entity<Order>().Property(c => c.OrderNo).IsRequired();
            modelBuilder.Entity<Category>().Property(c => c.Name).IsRequired();
            modelBuilder.Entity<Order>().Property(c => c.UserId).IsRequired();
            modelBuilder.Entity<Product>().Property(c => c.Price).IsRequired();
            modelBuilder.Entity<Order>().Property(c => c.TotalAmount).IsRequired();
            base.OnModelCreating(modelBuilder);
        }
    }
}