using AutoMapper;
using MoviesApi.DTOs;
using MoviesApi.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MoviesApi.Repositories
{
    public class GenreRepository : BaseRepository<Genre>
    {
        public GenreRepository(ApplicationDbContext context): base(context)
        {

        }

    }
}
