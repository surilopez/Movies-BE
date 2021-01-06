using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Movies_BE.Entities
{
    public class MoviesActors
    {
        public int MovieID { get; set; }
        public int ActorID { get; set; }
        public Movies movie { get; set; }
        public Actor actor { get; set; }

        [StringLength(maximumLength:100)]
        public string Character { get; set; }
        public int Order { get; set; }


    }
}
