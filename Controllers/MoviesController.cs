using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MoviesApi.DTOs;
using MoviesApi.Entities;
using MoviesApi.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MoviesApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MoviesController : ControllerCrudBase<Movie,MovieRepository>
    {
        public MoviesController(MovieRepository repository, ILogger<MoviesController> logger): base ( repository, logger)
        {

        }

        [HttpGet("PostGet")]
        public async Task<ActionResult<MoviePostGetDTO>> PostGet()
        {
            return await repository.AddGet();
        }

        [HttpGet("GetById/{id:int}")]
        public async Task<ActionResult<MovieDTO>> GetDTO (int id)
        {
            var dto = await repository.GetDTOById(id);
            if(dto == null)
            {
                return NotFound();
            }

            return dto;
        }

        [HttpPost]
        [Route("PostMovie")]

        public async Task<ActionResult> PostMovie([FromForm] MovieCreationDTO movieCreationDTO)
        {
            try
            {
                await repository.AddDTO(movieCreationDTO);
            }
            catch
            {
                return BadRequest();
            }

            return NoContent();
        }
    }
}
