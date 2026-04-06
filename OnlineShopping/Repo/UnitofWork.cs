using Microsoft.EntityFrameworkCore.Storage;
using OnlineShopping.Data;

namespace OnlineShopping.Repo
{
    public interface IUnitofWork
    {
        IEntityRep<Customer> CustomerRepo { get; }
        IEntityRep<Address> AddressRepo { get; }
        IEntityRep<Order> OrderRepo { get; }
        IEntityRep<OrderItem> OrderItemRepo { get; }
        IEntityRep<Product> ProductRepo { get; }
        IEntityRep<Category> CategoryRepo { get; }
        Task<int> SaveChanges();
        Task<IDbContextTransaction> BeginTransactionAsync();
    }

    public class UnitofWork : IUnitofWork
    {
        ShoppingContext context;

        public IEntityRep<Customer> CustomerRepo { get; }
        public IEntityRep<Address> AddressRepo { get; }
        public IEntityRep<Order> OrderRepo { get; }
        public IEntityRep<OrderItem> OrderItemRepo { get; }
        public IEntityRep<Product> ProductRepo { get; }
        public IEntityRep<Category> CategoryRepo { get; }

        public UnitofWork(ShoppingContext _context)
        {
            context = _context;
            CustomerRepo = new EntityRepo<Customer>(context);
            AddressRepo = new EntityRepo<Address>(context);
            OrderRepo = new EntityRepo<Order>(context);
            OrderItemRepo = new EntityRepo<OrderItem>(context);
            ProductRepo = new EntityRepo<Product>(context);
            CategoryRepo = new EntityRepo<Category>(context);
        }

        public async Task<int> SaveChanges()
        {
            return await context.SaveChangesAsync();
        }

        public async Task<IDbContextTransaction> BeginTransactionAsync()
        {
            return await context.Database.BeginTransactionAsync();
        }
    }
}