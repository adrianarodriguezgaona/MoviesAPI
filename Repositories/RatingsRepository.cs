using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using MoviesApi.DTOs;
using MoviesApi.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MoviesApi.Repositories
{
    public class RatingsRepository : MappingRepository<Rating>
    {
        private readonly UserManager<IdentityUser> userManager;
        private static HttpContext _httpContext => new HttpContextAccessor().HttpContext;
        public RatingsRepository(ApplicationDbContext context, IMapper mapper, UserManager<IdentityUser> userManager) : base(context,mapper)
        {
            this.userManager = userManager;
        }

        public async Task<Rating> AddDTO (RatingDTO ratingDTO )
        {
            Rating rating = new Rating();
            var email = _httpContext.User.Claims.FirstOrDefault(x => x.Type == "email").Value;
            var user = await userManager.FindByEmailAsync(email);
            var userId = user.Id;

            Rating currentRate = await applicationDb.Ratings
                .FirstOrDefaultAsync(x => x.MovieId == ratingDTO.MovieId && x.UserId == userId);
            if(currentRate == null)
            {
                rating.MovieId = ratingDTO.MovieId;
                rating.Rate = ratingDTO.Rating;
                rating.UserId = userId;
                return await Add(rating);
            }
            else
            {
                currentRate.Rate = ratingDTO.Rating;
                return await Update(currentRate);
            }
            
        } 
    }
}
