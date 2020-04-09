using System;
using System.Linq;
using System.Linq.Expressions;
using DataLayer.Contract;
using Microsoft.EntityFrameworkCore;

namespace DataLayer.Repository
{
    public abstract class RepositoryBase<T>: IRepositoryBase<T> where T : class
    {
        protected readonly FilmContext Context;

        protected RepositoryBase(FilmContext context)
        {
            Context = context;
        }

        public IQueryable<T> GetAll()
        {
            return Context.Set<T>().AsNoTracking();
        }

        public IQueryable<T> Find(Expression<Func<T, bool>> expression)
        {
            return Context.Set<T>().Where(expression).AsNoTracking();
        }

        public void Create(T entity)
        {
            Context.Set<T>().Add(entity);
        }

        public void Update(T entity)
        {
            Context.Set<T>().Update(entity);
        }

        public void Delete(T entity)
        {
            Context.Set<T>().Remove(entity);
        }

        public void Save()
        {
            Context.SaveChanges();
        }
    }
}