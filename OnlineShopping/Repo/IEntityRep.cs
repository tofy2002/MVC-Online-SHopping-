using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace OnlineShopping.Repo
{
    public interface IEntityRep<T>
    {
        Task<List<T>> GetAll(string? navprop=null);
        Task<T> GetById(int id);
        Task Add(T entity);
        void Delete(int id);
        void Update(T entity);
        //Task<int> SaveChanges();

       Task <List<T>> FindAll(Expression<Func<T, bool>> predicate);
        IQueryable<T> FindAllQueryable(Expression<Func<T, bool>> predicate);
     

    }
}
