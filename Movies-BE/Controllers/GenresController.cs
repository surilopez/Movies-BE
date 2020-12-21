using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore;
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
        private readonly ApplicationDBContext context;

        public GenresController(ILogger<GenresController> logger, ApplicationDBContext context)
        {
            
            this.logger = logger;
            this.context = context;
        }

        [HttpGet]        
        public async Task<ActionResult<List<Genres>>> Get()
        {

            return await context.Genres.ToListAsync();
        }

        [HttpGet("{Id:int}")]
        public async Task<ActionResult<Genres>> Get(int Id)
        {

            throw new NotImplementedException();
        }

        [HttpPost]
        public async Task<ActionResult> Post([FromBody] Genres genre)
        {

            context.Add(genre);
            await context.SaveChangesAsync();
            return NoContent();
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
