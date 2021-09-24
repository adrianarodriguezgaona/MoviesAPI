using MoviesApi.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MoviesApi.Entities
{
    public class Actor: EntityBase
    {
        [Required(ErrorMessage = "The field with name {0} is required")]
        [StringLength(60)]
        [FirstLetterUppercase]
        public string Name { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string Biography { get; set; }
        public string Picture { get; set; }
        public List<MoviesActors> MoviesActors { get; set; }
    }

}
