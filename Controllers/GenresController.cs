using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MoviesApi.DTOs;
using MoviesApi.Entities;
using MoviesApi.Repositories;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MoviesApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Policy = "IsAdmin")]
    public class GenresController : ControllerCrudBase<Genre, GenreRepository>
    {
        public GenresController(GenreRepository genreRepository, ILogger<GenresController> logger) : base(genreRepository, logger)
        {

        }
        [HttpGet]
        [Route("GetGenres")]
        [AllowAnonymous]
        public async Task<ActionResult<List<GenreDTO>>> GetGenres()
        {
            logger.LogInformation("getting all the genres");
            return await repository.GetGenre();
        }

        [HttpPost]
        [Route("PostGenre")]
        public async Task<ActionResult> PostDTO([FromBody] GenreCreationDTO genreCreationDTO)
        {
            await repository.AddDTO(genreCreationDTO);

            return NoContent();
        }
    }
}

