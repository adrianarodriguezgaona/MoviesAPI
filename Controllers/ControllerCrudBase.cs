using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MoviesApi.DTOs;
using MoviesApi.Entities;
using MoviesApi.Helpers;
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
        public  async Task<ActionResult<List<T>>> Get([FromQuery] PaginationDTO paginationDTO)
        {
            var queryable = repository.GetAll().AsQueryable();
            await HttpContext.InsertParametersPaginationInHeader(queryable);

            return await repository.ListAll(paginationDTO);
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

        [HttpPost]
        public async Task<ActionResult> Post([FromBody] T entity)

        {
            await repository.Add(entity);
           
            return NoContent();
        }


        [HttpPut("{id}")]
        public virtual async Task<ActionResult<T>> Put([FromRoute] int id , [FromBody] T entity)
        {
           if (id != entity.Id)
            {
                return BadRequest();
            }

            T updatedEntity = await repository.Update(entity);
         
            if (updatedEntity == null)
            {
                return NotFound();
            }

            return CreatedAtAction(nameof(Get), new { id = entity.Id }, entity);
        }

        [HttpDelete ("{id:int}")]
        public async Task<ActionResult<T>> Delete ([FromRoute] int id)
        {
            var deletedEntity = await repository.Delete(id);

            if (deletedEntity == null)
            {
                return NotFound();
            }
            return NoContent();
        }

    }
}
