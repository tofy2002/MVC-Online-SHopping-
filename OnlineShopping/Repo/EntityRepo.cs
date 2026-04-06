using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using OnlineShopping.Data;
using OnlineShopping.Repo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace OnlineShopping.Repo
{
    public class EntityRepo<T> : IEntityRep<T> where T : class
    {
        ShoppingContext context;
        DbSet<T> set;

        public EntityRepo(ShoppingContext _context)
        {
            context = _context;
            set = context.Set<T>();
        }

        public async Task Add(T entity)
        {
            await set.AddAsync(entity);
        }

        public void Delete(int id)
        {
            var entity= set.Find(id);
            if (entity == null)
                return;
            set.Remove(entity);
        }

        public async Task<List<T>> FindAll(Expression<Func<T, bool>> predicate)
        {
            return await set.Where(predicate).ToListAsync();
        }
        public IQueryable<T> FindAllQueryable(Expression<Func<T, bool>> predicate)
        {
            return set.Where(predicate);
        }

        public async Task<List<T>> GetAll(string? navprop =null)
        {
                if (navprop != null)
                {
                    return await set.Include(navprop).ToListAsync();
            }

            return await set.ToListAsync();
        }

        public async Task<T> GetById(int id)
        {
            return await set.FindAsync(id);
        }
       
        //public async Task<int> SaveChanges()
        //{
        //    return await context.SaveChangesAsync();
        //}

        public void Update(T entity)
        {
            set.Update(entity);
        }
    }
}
