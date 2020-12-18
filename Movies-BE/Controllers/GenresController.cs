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
using Movies_BE.Repos;

namespace Movies_BE.Controllers
{
    [Route("api/genres")]
    [ApiController]
    //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class GenresController : ControllerBase
    {
        private readonly IRepos repos;
        private readonly ILogger<GenresController> logger;

        public GenresController(IRepos repos, ILogger<GenresController> logger)
        {
            this.repos = repos;
            this.logger = logger;
        }

        [HttpGet]
        [HttpGet("AllGenres")]
        [HttpGet("/AllGenres")]
        [ServiceFilter(typeof(MyActionFilter))]
        public ActionResult<List<Genres>> Get()
        {
            logger.LogDebug("Getting Genres");
            return repos.GetGenres();
        }

        [HttpGet("{Id:int}")]
        public async Task<ActionResult<Genres>> Get(int Id)
        {

            logger.LogInformation($"Get Genre by ID: {Id}");

            var genre = await repos.GetGenreById(Id);

            if (genre == null)
            {
                throw new ApplicationException($"There are not genre to show with Id: { Id }");
                logger.LogWarning($"There are not genre to show with Id: {Id}");
                return NotFound();
            }
            return genre;
        }

        [HttpPost]
        public ActionResult Post([FromBody] Genres genre)
        {

            repos.AddGenre(genre);
            return NoContent();
        }
        [HttpPut]
        public ActionResult Put([FromBody] Genres genre)
        {

            return NoContent();
        }

        [HttpDelete]
        public ActionResult Delete()
        {
            return NoContent();
        }
    }
}
