using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Movies_BE.DTOs
{
    public class MoviesFiltersDTO
    {
        public int page { get; set; }
        public int recordsToShow { get; set; }
        public PaginationDTO paginationDTO
        { 
            get { return new PaginationDTO() { Page = page, recordsByPage = recordsToShow }; }
        }

        public string Title { get; set; }
        public int genreID { get; set; }
        public bool onTheaters { get; set; }
        public bool commingSoon { get; set; }
    }
}
