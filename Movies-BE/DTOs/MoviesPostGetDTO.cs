using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Movies_BE.Entities;

namespace Movies_BE.DTOs
{
    public class MoviesPostGetDTO
    {
        public List<GenresDTO> genres { get; set; }
        public List<TheaterDTO> theaters { get; set; }

    }
}
