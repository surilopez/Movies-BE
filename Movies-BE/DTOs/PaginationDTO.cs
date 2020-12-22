using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Movies_BE.DTOs
{
    public class PaginationDTO
    {
        public int Page { get; set; } = 1;
        public int recordsByPage { get; set; } = 10;

        private readonly int maxRecordsByPage = 50;


        public int RecordsByPage {
            get {
                return recordsByPage;
            }
            set {
                recordsByPage = (value > maxRecordsByPage) ? maxRecordsByPage : value;
            }
        }
    }
}
