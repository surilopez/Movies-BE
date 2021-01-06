﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using NetTopologySuite.Geometries;

namespace Movies_BE.Entities
{
    public class Theaters
    {

        public int id { get; set; }
        [Required]
        [StringLength(maximumLength:75)]
        public string Name { get; set; }
        public Point Location { get; set; }

        public List<MoviesTheaters> moviesTheaters { get; set; }


    }
}
