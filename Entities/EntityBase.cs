using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MoviesApi.Entities
{
    public class EntityBase
    {        
        public int Id { get; set; }
        [Required(ErrorMessage = "The field with name {0} is required")]
        [StringLength(60)]
        public string Name { get; set; }
    }
}
