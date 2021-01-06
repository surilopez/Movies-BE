using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Movies_BE.Entities
{
    public class MoviesTheaters
    {
        public int MovieID { get; set; }
        public int TheaterID { get; set; }

        public Movies movie { get; set; }

        public Theaters theater { get; set; }
    }
}
