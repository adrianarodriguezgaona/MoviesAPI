﻿using AutoMapper;
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
    [Authorize (AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Policy = "IsAdmin")]
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

        [HttpPost("searchByName")]
        public async Task<ActionResult<List<ActorsMovieDTO>>> SearchByName([FromBody] string name)
        {
            return await repository.SearchByName(name);
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
