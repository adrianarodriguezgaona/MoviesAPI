using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
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
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Policy = "IsAdmin")]
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

        [HttpGet("GetMovies")]
        [AllowAnonymous]
        public async Task<ActionResult<HomeDTO>> GetMovies()
        {
            return await repository.GetHome();           
        }

        [HttpGet("GetById/{id:int}")]
        [AllowAnonymous]
        public async Task<ActionResult<MovieDTO>> GetDTO (int id)
        {
            var dto = await repository.GetDTOById(id);
            if(dto == null)
            {
                return NotFound();
            }

            return dto;
        }

        [HttpGet ("PutGetById/{id:int}")]
        public async Task<ActionResult<MoviePutGetDTO>> PutGet(int id)
        {
            bool isAuthenticated = HttpContext.User.Identity.IsAuthenticated;
            string email = HttpContext.User.Claims.FirstOrDefault(x => x.Type == "email").Value;
            var moviePutGetDto = await repository.PutGet(id);
            if (moviePutGetDto == null)
            {
                return NotFound();
            }

            return moviePutGetDto;
        }

        [HttpPut("Edit/{Id}")]
        public async Task<ActionResult> Put(int id, [FromForm] MovieCreationDTO movieCreationDTO)
        {
            var editedMovie = await repository.Update(id, movieCreationDTO);
            if (editedMovie == null)
            {
                return NotFound();
            }

            return NoContent();
        }

        [HttpGet ("Filter")]
        [AllowAnonymous]
        public async Task<ActionResult<List<MovieDTO>>> Filter ([FromQuery] FilterMovieDTO filterMovieDTO)
        {
            var moviesQueryable = await repository.Filter(filterMovieDTO);
            await HttpContext.InsertParametersPaginationInHeader(moviesQueryable);

            return await repository.MoviesToDTO(moviesQueryable, filterMovieDTO);

        }

        [HttpPost]
        [Route("PostMovie")]

        public async Task<ActionResult<int>> PostMovie([FromForm] MovieCreationDTO movieCreationDTO)
        {
                       
           var newMovie = await repository.AddDTO(movieCreationDTO);
           
            if(newMovie == null)
            {
                return BadRequest();
            }

            return newMovie.Id;
        }
    }
}
