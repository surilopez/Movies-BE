using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Movies_BE.Entities
{
    public class MoviesGenres
    {
        public int MovieID { get; set; }
        public int GenreID { get; set; }

        public Movies movie { get; set; }

        public Genres genre { get; set; }
    }
}
