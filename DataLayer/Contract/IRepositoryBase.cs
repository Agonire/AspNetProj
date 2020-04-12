using System;
using System.Linq;
using System.Linq.Expressions;

namespace DataLayer.Contract
{
    public interface IRepositoryBase<T>
    {
        
        IQueryable<T> GetAll();
        IQueryable<T> Find(Expression<Func<T, bool>> expression);
        void Create(T entity);
        void Update(T newFilmData);
        void Delete(T entity);
        void Save();
    }
}