using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Movies_BE.DTOs
{
    public class AuthenticationResponse
    {
        public string token { get; set; }
        public DateTime expiration { get; set; }
    }
}
