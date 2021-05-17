using AutoMapper;
using MoviesApi.DTOs;
using MoviesApi.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MoviesApi.Repositories
{
    public class GenreRepository : MappingRepository<Genre>
    {
        public GenreRepository(ApplicationDbContext context, IMapper mapper): base(context, mapper)
        {

        }

        public virtual async Task<Genre> AddDTO(GenreCreationDTO genreCreationDTO)
        {
            var genre = _mapper.Map<Genre>(genreCreationDTO);

            return await Add(genre);
        }

    }
}
