using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Movies_BE.DTOs
{
    public class MovieDTO
    {
        public int id { get; set; }
               public string Title { get; set; }
        public string Info { get; set; }
        public string Trailer { get; set; }
        public bool onTheater { get; set; }
        public DateTime ReleaseDate { get; set; }
        public string Img { get; set; }
        public List<GenresDTO> genresDTO { get; set; }
        public List<MovieActorDTO> movieActorsDTO { get; set; }
        public List<TheaterDTO> theaterDTO { get; set; }
    }
}
