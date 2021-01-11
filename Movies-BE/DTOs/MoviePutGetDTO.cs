using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Movies_BE.DTOs
{
    public class MoviePutGetDTO
    {
        public MovieDTO movie { get; set; }
        public List<GenresDTO> selectedGenresDTO { get; set; }
        public List<GenresDTO> noselectedGenresDTO { get; set; }
        public List<MovieActorDTO> movieActorsDTO { get; set; }
        public List<TheaterDTO> selectedTheaterDTO { get; set; }
        public List<TheaterDTO> noSelectedTheaterDTO { get; set; }

    }
}
