using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Movies_BE.Entities
{
    public class Actor
    {
        public int id { get; set; }
        [Required]
        [StringLength(maximumLength:200)]
        public string Name { get; set; }
        public string Biography { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string Photo { get; set; }
    }
}
