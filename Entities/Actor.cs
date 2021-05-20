using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MoviesApi.Entities
{
    public class Actor: EntityBase
    {
        public DateTime DateOfBirth { get; set; }
        public string Biography { get; set; }
        public string Picture { get; set; }
        public List<MoviesActors> MoviesActors { get; set; }
    }

}
