using MoviesApi.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MoviesApi.Entities
{
    public class Genre : EntityBase
    {
        [Required(ErrorMessage = "The field with name {0} is required")]
        [StringLength(60)]
        [FirstLetterUppercase]
        public string Name { get; set; }
        public List<Movie> Movies { get; set; }

    }
}
