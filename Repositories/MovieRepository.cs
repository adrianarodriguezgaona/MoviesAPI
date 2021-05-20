using AutoMapper;
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
