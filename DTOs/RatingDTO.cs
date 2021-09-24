﻿using System;
using System.ComponentModel.DataAnnotations;


namespace MoviesApi.DTOs
{
    public class RatingDTO
    {   [Range(1,5)]
        public int Rating { get; set; }
        public int MovieId { get; set; }
    }
}
