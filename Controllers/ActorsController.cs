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
    [Route("api/[controller]")]
    [ApiController]
    public class ActorsController : ControllerCrudBase<Actor,ActorRepository>
    {

        public ActorsController(ActorRepository repository, ILogger<ActorsController> logger) : base (repository,logger)
        {

        }

        [HttpPost]
        [Route("PostActor")]

        public async Task<ActionResult> PostActor([FromForm] ActorCreationDTO actorCreationDTO)
        
        {
            await repository.AddDTO(actorCreationDTO);

            return NoContent();        
    }
}
