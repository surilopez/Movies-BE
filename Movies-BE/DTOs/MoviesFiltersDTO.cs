using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Movies_BE.DTOs
{
    public class MoviesFiltersDTO
    {
        public int Page { get; set; }
        public int recordsByPage { get; set; }
        public PaginationDTO paginationDTO
        { 
            get { return new PaginationDTO() { Page = Page, recordsByPage = recordsByPage }; }
        }

        public string Title { get; set; }
        public int genreID { get; set; }
        public bool onTheater { get; set; }
        public bool commingSoom { get; set; }
    }
}
