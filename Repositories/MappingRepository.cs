using AutoMapper;
using MoviesApi.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MoviesApi.Repositories
{
    public class MappingRepository<T> : BaseRepository<T> where T: EntityBase
    {
        protected readonly IMapper _mapper;

        public MappingRepository(ApplicationDbContext context, IMapper mapper): base (context)
        {
            _mapper = mapper;
        }

    }
}
