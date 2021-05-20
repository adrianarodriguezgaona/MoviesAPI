

using NetTopologySuite.Geometries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MoviesApi.Entities
{
    public class MovieTheater : EntityBase
    {
        public Point Location { get; set; }
        public List<Movie> Movies { get; set; }
    }
}
