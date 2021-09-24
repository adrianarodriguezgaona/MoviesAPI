using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MoviesApi.DTOs;
using MoviesApi.Entities;
using MoviesApi.Repositories;
using System;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace MoviesApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RatingsController : ControllerCrudBase<Rating, RatingsRepository>
    {

        public RatingsController(RatingsRepository ratingsRepository, ILogger<RatingsController> logger) : base(ratingsRepository, logger)
        {

        }
        
        [HttpPost]
        [Route("postRate")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<ActionResult> PostRate([FromBody] RatingDTO ratingDTO)
        {
            await repository.AddDTO(ratingDTO);

            return NoContent();
        }

    }
}
