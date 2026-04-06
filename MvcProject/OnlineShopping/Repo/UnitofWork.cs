using OnlineShopping.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineShopping.Repo
{
    public interface IUnitofWork
    {
        EntityRepo<Customer> CustomerRepo { get; }
        EntityRepo<Address> AddressRepo { get; }
        EntityRepo<Order> OrderRepo { get; }
        EntityRepo<OrderItem> OrderItemRepo { get; }
        EntityRepo<Product> ProductRepo { get; }
        EntityRepo<Category> CategoryRepo { get; }
        Task<int> SaveChanges();


    }
    public class UnitofWork : IUnitofWork

    {
        ShoppingContext context;
        public EntityRepo<Customer> CustomerRepo { get; }

        public EntityRepo<Address> AddressRepo { get; }

        public EntityRepo<Order> OrderRepo { get; }
        public EntityRepo<OrderItem> OrderItemRepo { get; }

        public EntityRepo<Product> ProductRepo { get; }

        public EntityRepo<Category> CategoryRepo { get; }

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
    }
}
