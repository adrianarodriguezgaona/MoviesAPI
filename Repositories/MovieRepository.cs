using AutoMapper;
using Microsoft.EntityFrameworkCore;
using MoviesApi.DTOs;
using MoviesApi.Entities;
using MoviesApi.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MoviesApi.Repositories
{
    public class MovieRepository :MappingRepository<Movie>
    {
        private IFileStorageService fileStorageService;
        private string containerName = "movies";

        public MovieRepository(ApplicationDbContext applicationDbContext, IMapper mapper, IFileStorageService fileStorageService) : base (applicationDbContext, mapper)
        {
            this.fileStorageService = fileStorageService;
        }

        public async Task<MoviePostGetDTO> AddGet()
        {
            var movieTheaters = await applicationDb.MovieTheaters.OrderBy(mt => mt.Name).ToListAsync();
            var genres = await applicationDb.Genres.OrderBy(g => g.Name).ToListAsync();

            var movieTheatersDTO = _mapper.Map<List<MovieTheaterDTO>>(movieTheaters);
            var genresDTO = _mapper.Map<List<GenreDTO>>(genres);

            return new MoviePostGetDTO() { Genres = genresDTO, MovieTheaters = movieTheatersDTO };
        }

        public virtual async Task<Movie> AddDTO(MovieCreationDTO movieCreationDTO)
        {
            var movie = _mapper.Map<Movie>(movieCreationDTO);

            if (movieCreationDTO.Poster != null)
            {
                movie.Poster = await fileStorageService.SaveFile(containerName, movieCreationDTO.Poster);
            }

            AnnotateActorsOrder(movie);

            return await Add(movie);
        }

        private void AnnotateActorsOrder (Movie movie)
        {
            if (movie.MoviesActors != null)
            {
                for (int i = 0; i < movie.MoviesActors.Count; i++)
                {
                    movie.MoviesActors[i].Order = i;
                }
            }
        }
    }
}
