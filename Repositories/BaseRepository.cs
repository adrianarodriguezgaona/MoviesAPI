using Microsoft.EntityFrameworkCore;
using MoviesApi.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MoviesApi.Repositories
{
    public class BaseRepository<T> : IRepository<T> where T : EntityBase
    {
        protected readonly ApplicationDbContext applicationDb;

        public BaseRepository(ApplicationDbContext context)
        {
            applicationDb = context;
        }

        public virtual async Task<T> Add(T entity)
        {
            applicationDb.Set<T>().Add(entity);
            try
            {
                await applicationDb.SaveChangesAsync();
            }
            catch
            {
                return null;
            }
            return entity;
        }

        public Task<T> Delete(T entity)
        {
            throw new NotImplementedException();
        }

        public Task<T> Delete(int id)
        {
            throw new NotImplementedException();
        }

        public virtual IQueryable<T> GetAll()
        {
            return applicationDb.Set<T>().AsNoTracking();
        }

        public Task<T> GetById(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<List<T>> ListAll()
        {
            return await GetAll().ToListAsync();
        }

        public Task<T> Update(T entity)
        {
            throw new NotImplementedException();
        }
    }
}
