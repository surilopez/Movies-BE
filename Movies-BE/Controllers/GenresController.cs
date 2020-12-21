using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.Logging;
using Movies_BE.Entities;
using Movies_BE.Filters;


namespace Movies_BE.Controllers
{
    [Route("api/genres")]
    [ApiController]
    //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class GenresController : ControllerBase
    {
        
        private readonly ILogger<GenresController> logger;

        public GenresController(ILogger<GenresController> logger)
        {
            
            this.logger = logger;
        }

        [HttpGet]        
        public ActionResult<List<Genres>> Get()
        {

            return new List<Genres>() { new Genres() { id = 1, Name = "Comedy" } };
        }

        [HttpGet("{Id:int}")]
        public async Task<ActionResult<Genres>> Get(int Id)
        {

            throw new NotImplementedException();
        }

        [HttpPost]
        public ActionResult Post([FromBody] Genres genre)
        {

            throw new NotImplementedException(); ;
        }
        [HttpPut]
        public ActionResult Put([FromBody] Genres genre)
        {

            throw new NotImplementedException();
        }

        [HttpDelete]
        public ActionResult Delete()
        {
            throw new NotImplementedException();
        }
    }
}
