using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace Movies_BE.DTOs
{
    public class ActorAddDTO
    {
        [Required]
        [StringLength(maximumLength: 200)]
        public string Name { get; set; }
        public string Biography { get; set; }
        public DateTime DateOfBirth { get; set; }
        public IFormFile ActorImage { get; set; }
        //public string Photo { get; set; }
    }
}
