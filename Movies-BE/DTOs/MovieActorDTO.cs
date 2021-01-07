﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Movies_BE.DTOs
{
    public class MovieActorDTO
    {
        public int id { get; set; }
        public string Name { get; set; }
        public string Photo { get; set; }
        public string Character { get; set; }
        public int Order { get; set; }
    }
}
