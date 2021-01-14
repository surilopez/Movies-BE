using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace Movies_BE.Entities
{
    public class Rating
    {
        public int id { get; set; }
        [Range(1,5)]
        public int score { get; set; }
        public int movieId { get; set; }
        public string userId { get; set; }
        public Movies movie { get; set; }
        public IdentityUser user{ get; set; }
    }
}
