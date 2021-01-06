using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Movies_BE.Utilities;

namespace Movies_BE.DTOs
{
    public class MovieAddDTO
    {

        [Required]
        [StringLength(maximumLength: 300)]
        public string Title { get; set; }
        public string Info { get; set; }
        public string Trailer { get; set; }
        public bool onTheater { get; set; }
        public DateTime ReleaseDate { get; set; }
        public IFormFile Img { get; set; }

        [ModelBinder(BinderType= typeof(TypeBinder<List<int>>))]
        public List<int> GenresIDList { get; set; }

        [ModelBinder(BinderType = typeof(TypeBinder<List<int>>))]
        public List<int> TheatersIDList { get; set; }
        
        [ModelBinder(BinderType = typeof(TypeBinder<List<ActorMovieAddDTO>>))]
        public List<ActorMovieAddDTO> ActorsList { get; set; }



    }
}
