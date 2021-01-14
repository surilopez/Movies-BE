using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Movies_BE.DTOs
{
    public class RatingDTO
    {
        public int movieId { get; set; }
        [Range(1, 5)]
        public int score { get; set; }
    }
}
