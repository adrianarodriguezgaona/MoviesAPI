using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
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
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Policy = "IsAdmin")]
    public class MovieTheatersController : ControllerCrudBase<MovieTheater, MovieTheaterRepository>
    {
        public MovieTheatersController(MovieTheaterRepository repository, ILogger<MovieTheatersController> logger) : base(repository, logger)
        {

        }

        [HttpGet]
        [Route ("GetMovieTheaters")]

        public async Task<ActionResult<List<MovieTheaterDTO>>> GetMovieTheaters()
        {
            return await repository.ListAllMovieTheaters();
        }

        [HttpGet]
        [Route ("GetMovieTheater/{id:int}")]

        public async Task<ActionResult<MovieTheaterDTO>> GetMovieTheaterById ([FromRoute] int id)
        {            
            return await repository.GetMovieTheaterById(id);
        }

        [HttpPost]
        [Route ("PostMovieTheater")]

        public async Task<ActionResult> PostMovieTheater(MovieTheaterCreationDTO movieTheaterCreationDTO)
        {
            await repository.AddMovieTheater(movieTheaterCreationDTO);

            return NoContent();
        }

        [HttpPut ("EditMovieTheater/{id:int}")]
        public async Task<ActionResult>PutMovieTheater ([FromRoute] int id, MovieTheaterCreationDTO movieTheaterCreationDTO)
        {
            await repository.UpdateMovieTheater(id, movieTheaterCreationDTO);
            return NoContent();
        }


    }
}
