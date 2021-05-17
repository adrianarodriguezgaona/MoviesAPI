using AutoMapper;
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

    public class GenresController : ControllerCrudBase<Genre, GenreRepository>
    {
        public GenresController(GenreRepository genreRepository, ILogger<GenresController> logger) : base(genreRepository, logger)
        {
            
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

