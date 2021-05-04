using MoviesApi.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MoviesApi.Repositories
{
    public interface IRepository<T> where T : EntityBase
    {
        Task<T> GetById(int id);
        IQueryable<T> GetAll();
        Task<List<T>> ListAll();
        Task<T> Add(T entity);
        Task<T> Delete(T entity);
        Task<T> Delete(int id);
        Task<T> Update(T entity);


    }
}
