using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MoviesApi.Entities;
using MoviesApi.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MoviesApi.Controllers
{
    [ApiController]
    public class ControllerCrudBase<T, R> : ControllerBase
        where T:EntityBase
        where R:BaseRepository<T>
    {
        protected R repository;
        private readonly ILogger<ControllerCrudBase<T, R>> logger;

        public ControllerCrudBase(R repository, ILogger<ControllerCrudBase<T, R>> logger)
        {
            this.repository = repository;
            this.logger = logger;
        }


        [HttpGet]
        public virtual async Task<ActionResult<List<T>>> Get()
        {

            return await repository.ListAll();
        }
        
        [HttpGet("{id:int}")]
        public virtual async Task<ActionResult<T>> GetById(int id)
        {
            T entity = await repository.GetById(id);

            if (entity == null)
            {
                logger.LogWarning($"{entity} with Id {id} not found");
                return NotFound();
            }

            return entity;
        }

        //[HttpPut ("{id:int}")]
        //public virtual async Task<ActionResult<T>> 



    }
}
