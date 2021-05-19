using AutoMapper;
using MoviesApi.DTOs;
using MoviesApi.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MoviesApi.Repositories
{
    public class MovieTheaterRepository : MappingRepository<MovieTheater>
    {
        public MovieTheaterRepository(ApplicationDbContext applicationDb, IMapper mapper) : base (applicationDb, mapper)
        {
                
        }

        public async Task<List<MovieTheaterDTO>> ListAllMovieTheaters()
        {
            var movieTheaters = await ListAll();

            return  _mapper.Map<List<MovieTheaterDTO>>(movieTheaters);
        } 

        public async Task<MovieTheaterDTO> GetMovieTheaterById (int id)
        {
            var movieTheater = await GetById(id);

            return _mapper.Map<MovieTheaterDTO>(movieTheater);
        }

        public async Task<MovieTheater> AddMovieTheater(MovieTheaterCreationDTO movieTheaterCreationDTO)
        {
            var movieTheater = _mapper.Map<MovieTheater>(movieTheaterCreationDTO);

            return await Add(movieTheater);
        }
        public async Task<MovieTheater> UpdateMovieTheater(int id, MovieTheaterCreationDTO movieTheaterCreationDTO)
        {
            var movieTheater = await GetById(id);
            if (movieTheater == null)
            {
                return null;
            }
            movieTheater = _mapper.Map(movieTheaterCreationDTO, movieTheater);
            await applicationDb.SaveChangesAsync();

            return movieTheater;
        }

    }
}
