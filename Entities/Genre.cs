using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MoviesApi.Entities
{
    public class Genre : EntityBase
    {
        public List<Movie> Movies { get; set; }

    }
}
