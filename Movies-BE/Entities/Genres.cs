using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Movies_BE.Validation;

namespace Movies_BE.Entities
{
    public class Genres:IValidatableObject
    {
        public int id { get; set; }
        [Required]
        //[FirstLetterUppercase]
        public string Name { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {

            if (!string.IsNullOrEmpty(this.Name))
            {
                var firstLetter = this.Name[0].ToString();
                if (firstLetter != firstLetter.ToUpper())
                {
                   yield return new ValidationResult("First Letter must be Uppercase", new string[] {nameof(Name)});
                }

            }
        }
    }
}
