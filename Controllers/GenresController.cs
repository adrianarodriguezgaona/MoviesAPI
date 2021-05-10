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
        private readonly IMapper _mapper;
        public GenresController(GenreRepository genreRepository, ILogger<GenresController> logger, IMapper mapper) : base(genreRepository, logger)
        {
            _mapper = mapper;
        }

        [HttpPost]
        [Route("PostGenre")]
        public async Task<ActionResult> PostDTO([FromBody] GenreCreationDTO genreCreationDTO)
        {
            var genre = _mapper.Map<Genre>(genreCreationDTO);
            await repository.Add(genre);
            return NoContent();
        }
    }
}

