using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Movies_BE.Validation;

namespace Movies_BE.DTOs
{
    public class GenresAddDTO
    {
        [Required]
        [StringLength(maximumLength: 50)]
        [FirstLetterUppercase]
        public string Name { get; set; }
    }
}
