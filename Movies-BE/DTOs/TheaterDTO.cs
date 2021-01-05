﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Movies_BE.DTOs
{
    public class TheaterDTO
    {
        public int id { get; set; }
        public string Name { get; set; }
        
        public double Latitude { get; set; }
      
        public double Longitude { get; set; }
    }
}
