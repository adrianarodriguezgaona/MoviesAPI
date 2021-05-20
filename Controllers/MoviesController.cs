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

        [HttpPost]
        [Route("PostMovie")]

        public async Task<ActionResult> PostActor([FromForm] MovieCreationDTO movieCreationDTO)

        {
            await repository.AddDTO(movieCreationDTO);

            return NoContent();
        }
    }
}
