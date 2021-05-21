using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MoviesApi.Entities
{
    public class Movie: EntityBase
    {
        public string Summary { get; set; }
        public string Trailer { get; set; }
        public bool InTheaters { get; set; }
        public DateTime ReleaseDate { get; set; }
        public string Poster { get; set; }
        public List<Genre> Genres { get; set; }
        public List<MovieTheater> MovieTheaters { get; set; }
        public List<MoviesActors> MoviesActors { get; set; }


        public Movie()
        {
            Genres = new List<Genre>();
            MovieTheaters = new List<MovieTheater>();
            MoviesActors = new List<MoviesActors>();
        }
    }

}
