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

        [HttpPut]
        [Route("EditActor/{id:int}")]

        public async Task<ActionResult> EditActor([FromRoute] int id, [FromForm] ActorCreationDTO actorCreationDTO)

        {
            await repository.EditDTO(id, actorCreationDTO);

            return NoContent();
        }

        [HttpDelete]
        [Route("DeleteActor/{id:int}")]

        public async Task<ActionResult> DeleteActor ([FromRoute] int id)
        {
            await repository.DeleteActor(id);

            return NoContent();          
        }
    }
}
