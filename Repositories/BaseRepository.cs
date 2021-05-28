using Microsoft.EntityFrameworkCore;
using MoviesApi.DTOs;
using MoviesApi.Entities;
using MoviesApi.Helpers;
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
            catch (Exception ex)
            {
                Console.WriteLine("{0} Exception caught.", ex);
            }
            return entity;
        }

        public virtual async Task<T> Attach(T entity)
        {
            applicationDb.Set<T>().Attach(entity);
            try
            {
                await applicationDb.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine("{0} Exception caught.", ex);
            }
            return entity;
        }


        public async Task<T> Delete(T entity)
        {
            applicationDb.Set<T>().Remove(entity);
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

        public async Task<T> Delete(int id)
        {
            var entity = await GetById(id);
            if (entity == null)
            {
                return null;
            }
            return await Delete(entity);
        }

        public virtual IQueryable<T> GetAll()
        {
            return applicationDb.Set<T>().AsNoTracking();
        }

        public async  Task<T> GetById(int id)
        {
           return await applicationDb.Set<T>().FirstOrDefaultAsync(t => t.Id == id);           
        }

       
        public async Task<List<T>> ListAll(PaginationDTO paginationDTO)
        {
            return await GetAll().OrderBy(e => e.Name).Paginate(paginationDTO).ToListAsync();
        }

        public async Task<List<T>> ListAll()
        {
            return await GetAll().OrderBy(e => e.Name).ToListAsync();
        }

        public async Task<T> Update(T entity)
        {
            applicationDb.Entry(entity).State = EntityState.Modified;
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
    }
}
