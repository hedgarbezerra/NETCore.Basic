using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Linq.Expressions;

namespace NETCore.Basic.Domain.Interfaces
{
    public interface IRepository<T> where T : class
    {
        T Add(T obj);
        T Update(T obj);
        void Delete(T obj);
        void Dispose();
        DbContext GetDbContext();
        void SaveChanges();
        IQueryable<T> Get();
        T Get(int id);
        IQueryable<T> Get(Expression<Func<T, bool>> filter = null);
        IQueryable<T> Get(Expression<Func<T, bool>> filter = null, Expression<Func<T, object>> order = null, int? count = 0, int? skip = 0, bool reverse = false);

    }
}
