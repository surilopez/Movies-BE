using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Movies_BE.DTOs
{
    public class LandingPageDTO
    {
        public List<MovieDTO> onTheaters { get; set; }
        public List<MovieDTO> commingSoom{ get; set; }
    }
}
