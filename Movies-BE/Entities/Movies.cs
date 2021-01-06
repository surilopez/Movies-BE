using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Movies_BE.Entities
{
    public class Movies
    {
        public int id { get; set; }
        [Required]
        [StringLength(maximumLength:300)]
        public string Title { get; set; }
        public string Info { get; set; }
        public string Trailer { get; set; }
        public bool onTheater { get; set; }
        public DateTime ReleaseDate { get; set; }
        public string Img { get; set; }
        public List<MoviesActors> moviesActors{ get; set; }
        public List<MoviesGenres> moviesGenres{ get; set; }
        public List<MoviesTheaters> moviesTheaters{ get; set; }


    }
}
